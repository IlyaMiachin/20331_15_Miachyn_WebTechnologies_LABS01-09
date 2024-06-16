using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;

namespace Miachyn.Blazor.Services
{
    public class ApiFurnitureService(HttpClient Http) : IFurnitureService<Furniture>
    {
        List<Furniture> _furniture;
        int _currentPage = 1;
        int _totalPages = 1;
        public IEnumerable<Furniture> Products => _furniture;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;
        public event Action ListChanged;
        public async Task GetProducts(int pageNo, int pageSize)
        {
            // Url сервиса API
            var uri = Http.BaseAddress.AbsoluteUri;
            // Данные для Query запроса
            var queryData = new Dictionary<string, string>
            {
                { "pageNo", pageNo.ToString() },
                {"pageSize", pageSize.ToString() }
            };
            var query = QueryString.Create(queryData);
            // Отправить запрос http
            var result = await Http.GetAsync(uri + query.Value);
            // В случае успешного ответа
            if (result.IsSuccessStatusCode)
            {
                // Получить данные из ответа
                var responseData = await result.Content
                .ReadFromJsonAsync<ResponseData<ListModel<Furniture>>>();
                // Обновить параметры
                _currentPage = responseData.Data.CurrentPage;
                _totalPages = responseData.Data.TotalPages;
                _furniture = responseData.Data.Items;
                ListChanged?.Invoke();
            }
            // В случае ошибки
            else
            {
                _furniture = null;
                _currentPage = 1;
                _totalPages = 1;
            }
        }
    }
}


