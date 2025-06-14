import axios, { AxiosError } from "axios";
import notificationService from './notificationService'

export const httpService = {

    GET: function (URL: string, Data: object) {
        const data = Data;
        return axios.get(URL, { params: data, headers: this.GetHeader() })
            .then((response) => response.data)
            .catch(this.ErrorMessage);
    },

    POST: function (URL: string, Data: object) {
        const data = JSON.stringify(Data);
        return axios.post(URL, data, { headers: this.GetHeader() })
            .then((response) => response.data)
            .catch(this.ErrorMessage);
    },

    PUT: function (URL: string, Data: object) {
        const data = JSON.stringify(Data);
        return axios.put(URL, data, { headers: this.GetHeader() })
            .then((response) => response.data)
            .catch(this.ErrorMessage);
    },

    DELETE: function (URL: string, Data: object) {
        const data = Data;
        return axios.get(URL, { params: data, headers: this.GetHeader() })
            .then((response) => response.data)
            .catch(this.ErrorMessage);            ;
    },

    UploadFile: function (URL: string, Data: FormData) {
        return axios.post(URL, Data, { headers: { 'Content-Type': 'multipart/form-data' }})
            .then((response) => response.data)
            .catch(this.ErrorMessage);
    },

    GenerateFile: function (URL: string, Data: object) {
        return axios.get(URL, { params: Data, responseType: 'blob', headers: this.GetHeader() })
            .then((response) => response.data)
            .catch(this.ErrorMessage);
    },

    ErrorMessage: function (error: AxiosError) {
        if (error === null) {
            notificationService.Error('Unrecoverable error!! Error is null!');
        } else if (axios.isAxiosError(error)) {
            // Here we have a type guard check, error inside this if will be treated as AxiosError
            const response = error.response;
            const request = error.request;

            if (error.code === 'ERR_NETWORK') {
                notificationService.Error('Connection problems..');
            } else if (error.code === 'ERR_CANCELED') {
                notificationService.Error('Connection canceled..');
            } else if (response) {
                // The request was made and the server responded with a status code that falls out of the range of 2xx
                const statusCode = response?.status;

                if (statusCode === 420) {
                    notificationService.Error(response.data);
                } else if (statusCode === 500) {
                    notificationService.Error(response.statusText);
                } else if (statusCode === 404) {
                    if (response.data === "") {
                        notificationService.Error(response.statusText);
                    } else {
                        notificationService.Error(response.data);
                    }
                } else if (statusCode === 401) {
                    if (response.data === "") {
                        notificationService.Error('Please login to access this resource');
                    } else {
                      //  notificationService.Error(response.data);
                        notificationService.Error(response.data.title); 
                    }
                    // Redirect user to login
                } else if (statusCode === 400) {
                    if (response.data === "") {
                        notificationService.Error(response.statusText);
                    } else {
                        //  notificationService.Error(response.data);
                        notificationService.Error(response.data.message); 
                    }
                    // Redirect user to login
                } else {
                    if (response.data === "") {
                        notificationService.Error(response.statusText);
                    } else {
                        notificationService.Error(response.data);
                    }
                }
            } else if (request) {
                notificationService.Error('The request was made but no response was received');
                // The request was made but no response was received
            }
        }

        return Promise.reject(error);
    },

    GetHeader: function () {
        let headers = {}; 
        const _authenticationToken = localStorage.getItem("AuthenticationToken");

        headers = {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${_authenticationToken}`,
        };

        return headers;
    }

};
