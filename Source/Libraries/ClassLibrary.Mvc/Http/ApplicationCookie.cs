using ClassLibrary.Common.Encryption;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClassLibrary.Mvc.Http
{
    public class ApplicationCookie
    {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;
        private readonly byte[] _encryptionKey;
        private readonly CookieOptions _cookieOptions;

        public ApplicationCookie(IHttpContextAccessor httpContextAccessor, byte[] encryptionKey)
        {
            _request = httpContextAccessor.HttpContext?.Request ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _response = httpContextAccessor.HttpContext?.Response ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
            _cookieOptions = GetDefaultCookieOptions();
        }

        public ApplicationCookie(IHttpContextAccessor httpContextAccessor, byte[] encryptionKey, CookieOptions cookieOptions)
        {
            _request = httpContextAccessor.HttpContext?.Request ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _response = httpContextAccessor.HttpContext?.Response ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
            _cookieOptions = cookieOptions ?? throw new ArgumentNullException(nameof(cookieOptions));
        }

        public string GetValue(string cookieKey, string key)
        {
            string value = string.Empty;
            string cookieValue = _request.Cookies[cookieKey.Clean()] ?? string.Empty;

            if (!string.IsNullOrEmpty(cookieValue))
            {
                cookieValue = AESGCM.SimpleDecrypt(cookieValue, _encryptionKey) ?? string.Empty;
                Dictionary<string, string> encryptedForm = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieValue) ?? new();
                if (encryptedForm.ContainsKey(key.Clean()))
                    value = AESGCM.SimpleDecrypt(encryptedForm[key.Clean()], _encryptionKey) ?? string.Empty;
            }

            return value;
        }

        public Dictionary<string, string> GetAllValues(string cookieKey)
        {
            Dictionary<string, string> form = new();
            string cookieValue = _request.Cookies[cookieKey.Clean()] ?? string.Empty;

            if (!string.IsNullOrEmpty(cookieValue))
            {
                cookieValue = AESGCM.SimpleDecrypt(cookieValue, _encryptionKey);
                Dictionary<string, string> encryptedForm = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieValue) ?? new();
                foreach(var item in encryptedForm)
                    form.Add(item.Key, AESGCM.SimpleDecrypt(item.Value, _encryptionKey));
            }

            return form;
        }

        public void SetValue(string cookieKey, string key, string value)
        {
            Dictionary<string, string> form = new();
            string cookieValue = _request.Cookies[cookieKey.Clean()] ?? string.Empty;

            if (!string.IsNullOrEmpty(cookieValue))
            {
                cookieValue = AESGCM.SimpleDecrypt(cookieValue, _encryptionKey);
                form = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieValue) ?? new();
            }

            if (form.ContainsKey(key.Clean()))
                form.Remove(key.Clean());

            form.Add(key.Clean(), AESGCM.SimpleEncrypt(value.Clean(), _encryptionKey));

            _response.Cookies.Append(cookieKey.Clean(), AESGCM.SimpleEncrypt(JsonConvert.SerializeObject(form), _encryptionKey), _cookieOptions);
        }

        public void SetAllValues(string cookieKey, Dictionary<string, string> values)
        {
            Dictionary<string, string> form = new();
            foreach (var item in values)
            {
                form.Add(item.Key.Clean(), AESGCM.SimpleEncrypt(item.Value.Clean(), _encryptionKey));
            }

            _response.Cookies.Append(cookieKey.Clean(), AESGCM.SimpleEncrypt(JsonConvert.SerializeObject(form), _encryptionKey), _cookieOptions);
        }

        public void Remove(string cookieKey)
        {
            _response.Cookies.Delete(cookieKey.Clean());
        }

        private static CookieOptions GetDefaultCookieOptions()
        {
            CookieOptions option = new()
            {
               HttpOnly = true,
               Secure = true,
               SameSite = SameSiteMode.Strict,
               Expires = DateTime.MinValue,
            };

            return option;
        }
    }
}
