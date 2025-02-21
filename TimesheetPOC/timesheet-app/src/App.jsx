
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import TimesheetPage from "./pages/TimesheetPage";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";

const App = () => {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/timesheets" element={<TimesheetPage />} />
      </Routes>
      <Footer />
    </Router>
  );
};

export default App;
