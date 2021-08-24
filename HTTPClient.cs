using System.Net.Http;
using System.Net.Http.Headers;
//using Newtonsoft.Json;

public void Init()
{
    _client = new HttpClient();

    _client.BaseAddress = new Uri("https://localhost:44374/");
    _client.DefaultRequestHeaders.Accept.Clear();
    _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
}

private async void CreateUserAsync()
{
    var arg = new UserArg(Name, Age);
    HttpResponseMessage response = await _client.PostAsJsonAsync("api/user/create", arg);
    response.EnsureSuccessStatusCode();

    var user = await response.Content.ReadAsAsync<UserDto>();
}

private async void UpdateUserAsync()
{
    var arg = new UserArg(Name, Age) { Id = SelectedUser.Id };
    HttpResponseMessage response = await _client.PutAsJsonAsync("api/user/update", arg);
    response.EnsureSuccessStatusCode();

    var user = await response.Content.ReadAsAsync<UserDto>();
}

private async void DeleteUserAsync()
{
    HttpResponseMessage response = await _client.DeleteAsync($"api/user/{SelectedUser.Id}");
    ServerResponse = response.StatusCode.ToString();
}

private async void GetUsersAsync()
{
    HttpResponseMessage response = await _client.GetAsync("api/user/all");
    if (response.IsSuccessStatusCode)
    {
        var users = await response.Content.ReadAsAsync<IEnumerable<UserDto>>();
        // var users = await response.Content.ReadAsStringAsync();
        //var deserilizedUsers = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(users);
    }
}

private readonly HttpClient _client;