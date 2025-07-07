using Application.DTOs.AuthorDTO;
using Application.DTOs.BookDTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            BookMappingRules();
            AuthorMappingRules();
        }

        public void BookMappingRules()
        {
            CreateMap<BookCreateDTO, Book>()
                   .ForMember(dest => dest.Authors,
                              opt => opt.MapFrom(src => src.AuthorsId
                              .Select(x => new Author() { Id = x })))
                   .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PublishDate)));

            CreateMap<BookUpdateDTO, Book>()
                   //.ForMember(dest => dest.Authors,
                   //           opt => opt.MapFrom(src => src.AuthorsId
                   //           .Select(x => new Author() { Id = x })))
                   .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.PublishDate)));


            CreateMap<Book, BookGetDTO>()
                .ForMember(dest => dest.AuthorsId, 
                opt => opt.MapFrom(src => src.Authors.Select(x=>x.Id)));
        }

        public void AuthorMappingRules()
        {
            CreateMap<Author, AuthorGetDTO>()
                .ForMember(dest => dest.BooksId, 
                opt => opt.MapFrom(src => src.Books.Select(x => x.Id)));

            CreateMap<AuthorCreateDTO, Author>()
                .ForMember(dest=>dest.BirthDate, opt=>opt.MapFrom(src=>DateOnly.FromDateTime(src.BirthDate)));


            CreateMap<AuthorUpdateDTO, Author>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));


        }
    }
   
   
}
