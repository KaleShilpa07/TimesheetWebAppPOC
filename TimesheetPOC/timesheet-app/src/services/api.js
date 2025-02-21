import axios from 'axios';

const API_URL = "https://localhost:7199/api";

export const getTimesheets = async () => {
    const response = await axios.get(API_URL);
    return response.data;
};

export const addTimesheet = async (timesheet) => {
    const response = await axios.post(API_URL, timesheet);
    return response.data;
};

export const updateTimesheet = async (id, timesheet) => {
    const response = await axios.put(`${API_URL}/${id}`, timesheet);
    return response.data;
};

export const deleteTimesheet = async (id) => {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
};
