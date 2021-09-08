using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using Newtonsoft.Json;
using ClientService.Areas.Movie.Models;
using ClientService.Helpers.Comment;
using Microsoft.Extensions.Configuration;
using GraphQL.Client.Abstractions;

namespace ClientService.Helpers
{
    public class CommentHelper : ICommentHelper
    {
        private readonly GraphQLHttpClient client;

        public  CommentHelper(IConfiguration Configuration)
        {
            client = new GraphQLHttpClient(Configuration.GetValue<string>("GraphQlUrl"), new NewtonsoftJsonSerializer());
            client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Configuration.GetValue<string>("GraphQlSecret")}");
        }

        public async Task<CommentListModel> GetComments(string movie_title)
        {
            var request = new GraphQLRequest
            {
                Query = "query {activeCommentsByMovieTitle(movieTitle: " + $"\"{movie_title}\"" + ", isActive: true" + "){data{_id date content user like movieTitle}}}",
            };

            CommentListModel commentListModel = new CommentListModel { Comments = new List<CommentModel>() };

            var response = await client.SendQueryAsync<object>(request);
            var stringResponse = response.Data.ToString();

            dynamic dynamicData = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            var commentsData = dynamicData.activeCommentsByMovieTitle.data;

            if (commentsData.Count == 0)
            {
                return new CommentListModel { };
            }
            foreach (var comment in commentsData)
            {
                CommentModel modelToAdd = MapJsonDataToCommentModel(comment);
                commentListModel.Comments.Add(modelToAdd);
            }
            return commentListModel;

        }

        public CommentModel MapJsonDataToCommentModel(dynamic item)
        {
            return new CommentModel
            {
                _id = item._id,
                content = item.content,
                date = item.date,
                like = item.like,
                movieTitle = item.movieTitle,
                user = item.user
            };
        }

        public async Task<bool> AddComment(string movie_title, string username, string date, string content)
        {
            var request = new GraphQLRequest
            {
                Query = "mutation CreateComment{createComment(data:{user: " + $"\"{username}\"" + " content: " + $"\"{content}\"" + " date: " + $"\"{date}\"" + " like: 0" + " movieTitle: " + $"\"{movie_title}\"" + "isActive: false}){ _id content}}"
            };

            var response = await client.SendQueryAsync<object>(request);

            if (response.Data == null)
            {
                // buradaki hata mesajını handle etmek gerek
                // return response.Errors
                return false;
            }
            var stringResponse = response.Data.ToString();
            dynamic dynamicData = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            var addCommentsData = dynamicData.createComment.data;
            return true;
        }

        public async Task<dynamic> GetAllComments()
        {
            var request = new GraphQLRequest
            {
                Query = "query{allComments{data{_id date content isActive user like movieTitle}}}",
            };


            var response = await client.SendQueryAsync<object>(request);
            var stringResponse = response.Data.ToString();

            dynamic dynamicData = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            var allcommentsData = dynamicData.allComments.data;

            return allcommentsData;
        }

        public async Task<bool> ApproveComment(CommentModel commentModel)
        {
            var request = new GraphQLRequest
            {
                Query = "mutation {updateComment(id: " + $"\"{commentModel._id}\"" + "data:{user: " + $"\"{commentModel.user}\"" + " content: " + $"\"{commentModel.content}\"" + " date: " + $"\"{commentModel.date}\"" + " like: 0" + " movieTitle: " + $"\"{commentModel.movieTitle}\"" + "isActive: true}){ _id content}}"
            };
            var response = await client.SendQueryAsync<object>(request);
            if (response.Data != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DisableComment(CommentModel commentModel)
        {
            var request = new GraphQLRequest
            {
                Query = "mutation {updateComment(id: " + $"\"{commentModel._id}\"" + "data:{user: " + $"\"{commentModel.user}\"" + " content: " + $"\"{commentModel.content}\"" + " date: " + $"\"{commentModel.date}\"" + " like: 0" + " movieTitle: " + $"\"{commentModel.movieTitle}\"" + "isActive: false}){ _id content}}"
            };
            var response = await client.SendQueryAsync<object>(request);
            if (response.Data != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteComment(CommentModel commentModel)
        {
            var request = new GraphQLRequest
            {
                Query = "mutation {deleteComment(id: " + $"\"{commentModel._id}\"" + "){content}}"
            };
            var response = await client.SendQueryAsync<object>(request);
            var stringResponse = response.Data.ToString();
            dynamic dynamicData = JsonConvert.DeserializeObject<dynamic>(stringResponse);
            if (dynamicData.deleteComment != null)
            {
                return true;
            }
            return false;
        }
    }
}
