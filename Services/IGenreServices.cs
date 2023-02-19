﻿namespace MoviesAPI.Services
{
    public interface IGenreServices
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(byte id);
        Task<Genre> Add(Genre genre);

        Genre Update(Genre genre);
        Genre Delete(Genre genre);

        Task<bool> IsvalidGenre(byte id);
    }
}
