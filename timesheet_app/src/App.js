import React from "react";
import { Container } from "@mui/material";
import ExcelUploader from "../src/components/ExcelUploader";

const App = () => {
    return (
        <Container maxWidth="sm" sx={{ paddingTop: 5 }}>
            <ExcelUploader />
        </Container>
    );
};

export default App;
