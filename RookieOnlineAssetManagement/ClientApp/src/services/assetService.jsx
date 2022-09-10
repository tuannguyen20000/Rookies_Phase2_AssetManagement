import axios from "axios";

const API_URL = "/api";

const createAsset = (data) => {
    return axios.post(API_URL + "/assets/create-asset", data);
};
const getCategories = () => {
    return axios.get(API_URL + `/assets/get-all-categories`);
};
const createCategory = (data) => {
    return axios.post(API_URL + "/assets/create-category", data);
};
const getAsset = (id) => {
    return axios.get(API_URL + `/assets/get-update-detail/${id}`);
};
const updateAsset = (id, data) => {
    return axios.put(API_URL + `/assets/update/${id}`, data);
};
const getState = (id) => {
    return axios.get(API_URL + `/assets/get-state/${id}`);
};
const checkHistory = (id) => {
    return axios.get(API_URL + `/assets/checkHistory/${id}`);
};
const deleteAsset = (id) => {
    return axios.put(API_URL + `/assets/delete/${id}`);
};
const getAssetDetail = (id) => {
    return axios.get(API_URL + `/assets/${id}`);
};
const getHistory = (id) => {
    return axios.get(API_URL + `/assets/history/${id}`);
};
export default {
    createAsset,
    getCategories,
    createCategory,
    getAsset,
    updateAsset,
    getState,
    checkHistory,
    deleteAsset,
    getAssetDetail,
    getHistory,
};
