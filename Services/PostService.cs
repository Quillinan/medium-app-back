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

        private static void ValidateTitleAndContent(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Content))
            {
                throw new ArgumentException("Title and content are required.");
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
                Content = createPostRequest.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = EncryptAuthorId(createPostRequest.AuthorId)
            };

            ValidateTitleAndContent(post);

            await postRepository.AddPostAsync(post);
            return post;
        }

        public async Task<bool> UpdatePostAsync(int id, Post updatedPost)
        {
            var existingPost = await GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            ValidateTitleAndContent(updatedPost);

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
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
