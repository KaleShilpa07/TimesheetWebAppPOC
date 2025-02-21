import { Button, Container, Typography, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";

const Home = () => {
  const navigate = useNavigate();

  return (
    <Container 
      maxWidth="md" 
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        minHeight: "100vh", // Full screen height
        textAlign: "center",
      }}
    >
      <Box>
        <Typography 
          variant="h3" 
          fontWeight="bold" 
          gutterBottom
          sx={{
            fontSize: { xs: "2rem", sm: "3rem", md: "3.5rem" }, // Dynamic font scaling
          }}
        >
          Welcome to Timesheet Management
        </Typography>
        
        <Typography 
          variant="h6" 
          color="textSecondary" 
          paragraph
          sx={{
            fontSize: { xs: "1rem", sm: "1.2rem" },
            maxWidth: "600px", 
            margin: "auto",
          }}
        >
          Track your work hours efficiently and manage your project tasks with ease.
        </Typography>

        <Button
          variant="contained"
          color="primary"
          size="large"
          sx={{
            mt: 3,
            px: 4,
            py: 1,
            fontSize: "1.2rem",
            transition: "all 0.3s ease",
            "&:hover": {
              backgroundColor: "#135ba1", // Darker shade on hover
            },
          }}
          onClick={() => navigate("/timesheets")}
        >
          Get Started
        </Button>
      </Box>
    </Container>
  );
};

export default Home;
