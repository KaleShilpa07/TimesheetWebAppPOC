
import { Container, Typography, Box, Link } from "@mui/material";

const Footer = () => {
  return (
    <Box
      component="footer"
      sx={{
        py: 3,
        px: 2,
        mt: "auto",
        backgroundColor: "#f8f9fa",
        textAlign: "center",
      }}
    >
      <Container maxWidth="md">
        <Typography variant="body2" color="textSecondary">
          &copy; {new Date().getFullYear()} Timesheet Management. All rights reserved.
        </Typography>
        <Box mt={1}>
          <Link href="/" color="inherit" sx={{ mx: 1 }}>
            Home
          </Link>
          <Link href="/timesheets" color="inherit" sx={{ mx: 1 }}>
            Timesheets
          </Link>
          <Link href="/about" color="inherit" sx={{ mx: 1 }}>
            About
          </Link>
          <Link href="/contact" color="inherit" sx={{ mx: 1 }}>
            Contact
          </Link>
        </Box>
      </Container>
    </Box>
  );
};

export default Footer;