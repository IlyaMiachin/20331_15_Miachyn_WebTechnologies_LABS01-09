using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;
using System.Net.Http;
using System.Text.Json;

namespace Miachyn.UI.Services.FurnitureService
{
    public class ApiFurnitureService(HttpClient httpClient) : IFurnitureService
    {
        public async Task<ResponseData<Furniture>> CreateFurnitureAsync(Furniture product, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            // Подготовить объект, возвращаемый методом
            var responseData = new ResponseData<Furniture>();
            // Послать запрос к API для сохранения объекта
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, product);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Не удалось создать объект: {response.StatusCode}";
                return responseData;
            }
            // Если файл изображения передан клиентом
            if (formFile != null)
            {
                // получить созданный объект из ответа Api-сервиса
                var furniture = await response.Content.ReadFromJsonAsync<Furniture>();
                // создать объект запроса
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}/{furniture.Id}")
                };
                // Создать контент типа multipart form-data
                var content = new MultipartFormDataContent();
                // создать потоковый контент из переданного файла
                var streamContent = new StreamContent(formFile.OpenReadStream());
                // добавить потоковый контент в общий контент по именем "image"
                content.Add(streamContent, "image", formFile.FileName);
                // поместить контент в запрос
                request.Content = content;
                // послать запрос к Api-сервису
                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"Не удалось сохранить изображение: {response.StatusCode}";
                }
            }
            return responseData;
        }
        public Task DeleteFurnitureAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<Furniture>> GetFurnitureByIdAsync(int id)
        {
            var apiUrl = $"{httpClient.BaseAddress.AbsoluteUri}{id}";
            var response = await httpClient.GetFromJsonAsync<Furniture>(apiUrl);
            return new ResponseData<Furniture>() { Data = response };
        }

        public async Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;

            var queryData = new Dictionary<string, string>()
            {
                { "pageNo", pageNo.ToString() }
            };

            if (!String.IsNullOrEmpty(categoryNormalizedName))
            {
                queryData.Add("category", categoryNormalizedName);
            }
            var query = QueryString.Create(queryData);
            var result = await httpClient.GetAsync(uri + query.Value);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Furniture>>>();
            };
            var response = new ResponseData<ListModel<Furniture>>
            { Success = false, ErrorMessage = "Ошибка чтения API" };
            return response;
        }

        public Task UpdateFurnitureAsync(int id, Furniture product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
