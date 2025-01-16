import axios from "axios";

// Axios örneği oluşturuluyor
const api = axios.create({
  baseURL: "http://localhost:5092/api", // Backend'inizin ana URL'sini buraya yazın
});

// İstekleri token ile yapmak isteyip istemediğinizi belirlemek için bir flag (işaret) ekleniyor
api.interceptors.request.use(
  (config) => {
    // Eğer config.headers'da `useToken` true ise token'ı ekleyin
    if (config.headers.useToken) {
      const token = localStorage.getItem("token");

      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    }

    // `useToken` header'ını kaldırarak backend'e gereksiz veri göndermiyoruz
    delete config.headers.useToken;

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;
