using System.Security.Cryptography;
using System.Text;
using medium_app_back.Models;
using medium_app_back.Repositories;
using medium_app_back.Requests;

namespace medium_app_back.Services
{
    public class PostService(PostRepository postRepository, string encryptionKey)
    {
        private readonly PostRepository postRepository = postRepository;

        private static void ValidateField(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"The {fieldName} is required.");
            }
        }

        private string EncryptAuthorId(string authorId)
        {
            var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            if (keyBytes.Length != 32)
            {
                throw new ArgumentException("A chave deve ter 32 bytes para AES-256.");
            }

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.Mode = CipherMode.CBC;
            aes.GenerateIV();

            var iv = aes.IV;
            using var encryptor = aes.CreateEncryptor(aes.Key, iv);

            var plainTextBytes = Encoding.UTF8.GetBytes(authorId);
            var encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            var resultBytes = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, resultBytes, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, resultBytes, iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(resultBytes);
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await postRepository.GetAllPostsAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await postRepository.GetPostByIdAsync(id);
        }

        public async Task<List<Post>> GetPostsByAuthorIdAsync(string authorId)
        {
            var encryptedAuthorId = EncryptAuthorId(authorId);
            return await postRepository.GetPostsByAuthorIdAsync(encryptedAuthorId);
        }

        public async Task<Post> AddPostAsync(CreatePostRequest createPostRequest)
        {

            var post = new Post
            {
                Title = createPostRequest.Title,
                Subtitle = createPostRequest.Subtitle,
                Content = createPostRequest.Content,
                CoverImage = createPostRequest.CoverImage,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = EncryptAuthorId(createPostRequest.AuthorId),
                AuthorName = createPostRequest.AuthorName
            };

            ValidateField(post.Title, nameof(post.Title));
            ValidateField(post.Subtitle, nameof(post.Subtitle));
            ValidateField(post.Content, nameof(post.Content));

            await postRepository.AddPostAsync(post);
            return post;
        }

        public async Task<bool> UpdatePostAsync(int id, UpdatePostRequest updatedPost)
        {
            var existingPost = await GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(updatedPost.Title))
            {
                existingPost.Title = updatedPost.Title;
            }

            if (!string.IsNullOrWhiteSpace(updatedPost.Subtitle))
            {
                existingPost.Subtitle = updatedPost.Subtitle;
            }

            if (!string.IsNullOrWhiteSpace(updatedPost.Content))
            {
                existingPost.Content = updatedPost.Content;
            }

            if (!string.IsNullOrWhiteSpace(updatedPost.CoverImage))
            {
                existingPost.CoverImage = updatedPost.CoverImage;
            }

            existingPost.UpdatedAt = DateTime.UtcNow;

            await postRepository.UpdatePostAsync(existingPost);
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var existingPost = await GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            await postRepository.DeletePostAsync(existingPost);
            return true;
        }
    }
}
