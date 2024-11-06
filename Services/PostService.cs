// using System.Security.Cryptography;
// using System.Text;
using medium_app_back.Data;
using medium_app_back.Models;
using medium_app_back.Repositories;

namespace medium_app_back.Services
{
    public class PostService(PostRepository postRepository, AppDbContext context)
    {
        private readonly PostRepository postRepository = postRepository;
        private readonly AppDbContext _context = context;

        //  private readonly string encryptionKey = encryptionKey;

        // private string EncryptAuthorId(string authorId)
        // {
        //     var keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

        //     if (keyBytes.Length != 32)
        //     {
        //         throw new ArgumentException("A chave deve ter 32 bytes para AES-256.");
        //     }

        //     using var aes = Aes.Create();
        //     aes.Key = keyBytes;
        //     aes.Mode = CipherMode.CBC;
        //     aes.GenerateIV();

        //     var iv = aes.IV;
        //     using var encryptor = aes.CreateEncryptor(aes.Key, iv);

        //     var plainTextBytes = Encoding.UTF8.GetBytes(authorId);
        //     var encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

        //     var resultBytes = new byte[iv.Length + encryptedBytes.Length];
        //     Buffer.BlockCopy(iv, 0, resultBytes, 0, iv.Length);
        //     Buffer.BlockCopy(encryptedBytes, 0, resultBytes, iv.Length, encryptedBytes.Length);

        //     return Convert.ToBase64String(resultBytes);
        // }
        private static async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile imageFile)
        {
            if (imageFile == null)
                return [];

            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<List<GetPostRequest>> GetAllPostsAsync()
        {
            var posts = await postRepository.GetAllPostsAsync();
            return posts.Select(ConvertToGetPostRequest).ToList();
        }

        public async Task<GetPostRequest?> GetPostByIdAsync(int id)
        {
            var post = await postRepository.GetPostByIdAsync(id);
            return post != null ? ConvertToGetPostRequest(post) : null;
        }

        public async Task<Post?> GetRawPostByIdAsync(int id)
        {
            return await postRepository.GetPostByIdAsync(id);
        }

        public async Task<List<Post>> GetPostsByAuthorIdAsync(string authorId)
        {
            return await postRepository.GetPostsByAuthorIdAsync(authorId);
        }

        public async Task<Post> AddPostAsync(CreatePostRequest createPostRequest)
        {
            var imageData = await ConvertImageToByteArrayAsync(createPostRequest.CoverImageData);

            var post = new Post
            {
                Title = createPostRequest.Title,
                Subtitle = createPostRequest.Subtitle,
                Content = createPostRequest.Content,
                CoverImageData = imageData,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AuthorId = createPostRequest.AuthorId,
                AuthorName = createPostRequest.AuthorName
            };

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post?> UpdatePostAsync(int postId, UpdatePostRequest updatedPost)
        {
            var existingPost = await _context.Posts.FindAsync(postId);
            if (existingPost == null)
            {
                return null;
            }

            if (updatedPost.CoverImageData != null)
            {
                var imageData = await ConvertImageToByteArrayAsync(updatedPost.CoverImageData);
                existingPost.CoverImageData = imageData;
            }




            existingPost.Title = updatedPost.Title ?? existingPost.Title;
            existingPost.Subtitle = updatedPost.Subtitle ?? existingPost.Subtitle;
            existingPost.Content = updatedPost.Content ?? existingPost.Content;
            existingPost.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingPost;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var existingPost = await GetRawPostByIdAsync(id);
            if (existingPost == null)
            {
                return false;
            }

            await postRepository.DeletePostAsync(existingPost);
            return true;
        }

        private static GetPostRequest ConvertToGetPostRequest(Post post)
        {
            string base64Image = Convert.ToBase64String(post.CoverImageData);
            string imageUrl = $"data:image/png;base64,{base64Image}";

            return new GetPostRequest
            {
                Id = post.Id,
                Title = post.Title,
                Subtitle = post.Subtitle,
                Content = post.Content,
                CoverImageUrl = imageUrl,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                AuthorId = post.AuthorId,
                AuthorName = post.AuthorName
            };
        }
    }
}
