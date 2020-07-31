using MydateAPI.Helpers;
using MydateAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MydateAPI.Repositories.Interfaces
{
    public interface IMyDateRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();

        //Task<IEnumerable<User>> GetUsers();

        //For padding
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<PagedList<User>> GetLikersLikees(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Like> GetLike(int userId, int recipientId);
        Task<Message> GetMessage(int id);
        Task<List<Message>> GetMessages(int userId, int recipientId);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);

        Task<IEnumerable<int>> GetUserLikes(int id, bool likers);
        Task<IEnumerable<User>> GetUserss(UserParams userParams);
    }
}
