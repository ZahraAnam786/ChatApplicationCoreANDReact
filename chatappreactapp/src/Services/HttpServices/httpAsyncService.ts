import axios, { AxiosError } from "axios";
import notificationService from './notificationService';

export const httpService = {
    GET: async function (URL: string, Data: object) {
        try {
            const response = await axios.get(URL, {
                params: Data,
                headers: this.GetHeader()
            });
            return response.data;
        } catch (error) {
            return this.ErrorMessage(error as AxiosError);
        }
    },

    POST: async function (URL: string, Data: object) {
        try {
            const response = await axios.post(URL, JSON.stringify(Data), {
                headers: this.GetHeader()
            });
            return response.data;
        } catch (error) {
            return this.ErrorMessage(error as AxiosError);
        }
    },

    PUT: async function (URL: string, Data: object) {
        try {
            const response = await axios.put(URL, JSON.stringify(Data), {
                headers: this.GetHeader()
            });
            return response.data;
        } catch (error) {
            return this.ErrorMessage(error as AxiosError);
        }
    },

    DELETE: async function (URL: string, Data: object) {
        try {
            const response = await axios.get(URL, {
                params: Data,
                headers: this.GetHeader()
            });
            return response.data;
        } catch (error) {
            return this.ErrorMessage(error as AxiosError);
        }
    },

    GenerateFile: async function (URL: string, Data: object) {
        try {
            const response = await axios.get(URL, {
                params: Data,
                responseType: 'blob',
                headers: this.GetHeader()
            });
            return response.data;
        } catch (error) {
            return this.ErrorMessage(error as AxiosError);
        }
    },

    ErrorMessage: function (error: AxiosError) {
        if (!error) {
            notificationService.Error('Unrecoverable error!! Error is null!');
        } else if (axios.isAxiosError(error)) {
            const response = error.response;
            const request = error.request;

            if (error.code === 'ERR_NETWORK') {
                notificationService.Error('Connection problems..');
            } else if (error.code === 'ERR_CANCELED') {
                notificationService.Error('Connection canceled..');
            } else if (response) {
                const statusCode = response?.status;

                if (statusCode === 420) {
                    notificationService.Error(response.data);
                } else if (statusCode === 500) {
                    notificationService.Error(response.statusText);
                } else if (statusCode === 404) {
                    notificationService.Error(response.data || response.statusText);
                } else if (statusCode === 401) {
                    notificationService.Error(response.data || 'Please login to access this resource');
                } else {
                    notificationService.Error(response.data || response.statusText);
                }
            } else if (request) {
                notificationService.Error('The request was made but no response was received');
            }
        }

        return Promise.reject(error);
    },

    GetHeader: function () {
        const _authenticationToken = localStorage.getItem("AuthenticationToken");

        return {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': _authenticationToken,
        };
    }
};
