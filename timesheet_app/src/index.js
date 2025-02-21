import React from 'react';
import ReactDOM from 'react-dom/client'; // Update import
import App from './App';

const root = ReactDOM.createRoot(document.getElementById('root')); // createRoot for React 18
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
