import { httpService } from './HttpServices/httpService'

export const authService = {

    baseURL: String(import.meta.env.VITE_API_BASE_URL)+'User/',

    FileUploadSevice: function (data: FormData) {
        return httpService.UploadFile(this.baseURL + 'uploadFile', data).then((response) => response);
    },

    LoginSevice: function (data: object) {
        return httpService.POST(this.baseURL + 'login', data).then((response) => {
            localStorage.setItem("AuthenticationToken", response.token)
            return response.userData
        });
    },

    RegisterSevice: function (data: object) {
         return httpService.POST(this.baseURL + 'register', data).then((response) => {
            localStorage.setItem("AuthenticationToken", response.token)
            return response.userData
        });
    },

    getCurrentUser: function () {
        return httpService.GET(this.baseURL + 'getCurrentUser', {}).then((response) => {
            return response.userData
        });
    },

     getAllUser: function () {
        return httpService.GET(this.baseURL + 'getAllUser', {}).then((response) => response);
    },

    getChatMessages: function (senderId: Number, receiverId: Number) {
        return httpService.GET(this.baseURL + 'getChatMessages', {
            senderId: Number(senderId),
            receiverId: Number(receiverId)
        }).then((response) => response);
    }
  
}
