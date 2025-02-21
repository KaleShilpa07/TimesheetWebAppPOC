import React, { useState } from "react";
import axios from "axios";
import { Button, CircularProgress, Typography, Box, Paper, Snackbar } from "@mui/material";
import MuiAlert from '@mui/material/Alert';

const ExcelUploader = () => {
  const [file, setFile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [openSnackbar, setOpenSnackbar] = useState(false);

  // Reference to the file input
  const fileInputRef = React.useRef();

  // Handle File Selection when a file is chosen (either dragged or selected)
  const handleFileChange = (e) => {
    const selectedFile = e.target.files ? e.target.files[0] : e.dataTransfer.files[0];
    if (selectedFile) {
      setFile(selectedFile);
      setErrorMessage("");
      setSuccessMessage("");
    }
  };

  // Handle the file drag and drop event
  const handleFileDrop = (e) => {
    e.preventDefault();
    const droppedFile = e.dataTransfer.files[0];
    if (droppedFile) {
      handleFileChange({ target: { files: [droppedFile] } });
    }
  };

  // Prevent the default behavior for dragover event
  const handleDragOver = (e) => {
    e.preventDefault();
  };

  // Handle File Upload (trigger file input dialog)
  const handleUpload = () => {
    fileInputRef.current.click();
  };

  // Handle File Import (Import Button)
  const handleImport = async () => {
    if (!file) {
      setErrorMessage("Please select an Excel file first.");
      setSuccessMessage("");
      return;
    }

    setLoading(true);
    try {
      const formData = new FormData();
      formData.append("file", file); // Appending the selected file

      // Send form data (multipart/form-data) to the backend API
      await axios.post("https://localhost:7199/api/ExcelImport/import", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        }
      });

      setSuccessMessage("Data imported successfully!");
      setErrorMessage("");
    } catch (error) {
      console.error("Error uploading file:", error);
      setSuccessMessage("");
      setErrorMessage("Failed to import data.");
    } finally {
      setLoading(false);
    }
  };

  // Cancel file selection
  const handleCancel = () => {
    setFile(null);
    setErrorMessage("");
    setSuccessMessage("");
  };

  // Snackbar close handler
  const handleSnackbarClose = () => {
    setOpenSnackbar(false);
  };

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        minHeight: "100vh",
      }}
    >
      <Paper sx={{ padding: 4, textAlign: "center", maxWidth: 600, width: "100%" }}>
        <Typography variant="h4" gutterBottom>
          Import Excel File
        </Typography>

        {/* Drag-and-drop area */}
        <Box
          sx={{
            border: "2px dashed #2196F3",
            padding: 2,
            marginBottom: 2,
            cursor: "pointer",
            backgroundColor: "#fafafa",
            transition: "background-color 0.3s ease",
          }}
          onDrop={handleFileDrop}
          onDragOver={handleDragOver}
        >
          <Typography variant="body1">
            {file ? (
              // Display the selected file name dynamically in the placeholder
              `${file.name} file selected.`
            ) : (
              <>
                Drag and drop an Excel file here, or{" "}
                <Button onClick={handleUpload} sx={{ textTransform: "none" }} color="primary">
                  Choose File
                </Button>
              </>
            )}
          </Typography>

          {/* Display error and success messages */}
          {errorMessage && <Typography variant="body2" color="error" marginTop={2}>{errorMessage}</Typography>}
          {successMessage && <Typography variant="body2" color="green" marginTop={2}>{successMessage}</Typography>}
        </Box>

        {/* File input (hidden) */}
        <input
          type="file"
          accept=".xlsx, .xls"
          onChange={handleFileChange}
          style={{ display: "none" }}
          ref={fileInputRef}
        />

        {/* Cancel Button */}
        <Box display="flex" justifyContent="space-between">
          <Button
            onClick={handleCancel}
            variant="outlined"
            color="secondary"
            sx={{ width: "45%", marginRight: "10px" }}
            disabled={!file}
          >
            Cancel
          </Button>

          {/* Import Data Button */}
          <Button
            onClick={handleImport}
            variant="contained"
            color="primary"
            sx={{ width: "45%" }}
            disabled={loading || !file}
          >
            {loading ? <CircularProgress size={24} color="inherit" /> : "Import Data"}
          </Button>
        </Box>
      </Paper>

      {/* Snackbar (Success/Error messages display) */}
      <Snackbar
        open={openSnackbar}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
      >
        <MuiAlert severity="error" onClose={handleSnackbarClose} sx={{ width: '100%' }}>
          {errorMessage || successMessage}
        </MuiAlert>
      </Snackbar>
    </Box>
  );
};

export default ExcelUploader;
