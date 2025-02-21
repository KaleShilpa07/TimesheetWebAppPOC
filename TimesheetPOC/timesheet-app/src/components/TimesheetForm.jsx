import PropTypes from "prop-types"; 
import { useState, useEffect } from "react";
import { Button, TextField, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material";

const TimesheetForm = ({ open, handleClose, handleSave, timesheet }) => {
    const [formData, setFormData] = useState({
        employeeID: "",
        projectID: "",
        date: "",
        hours: 0
    });

    useEffect(() => {
        if (timesheet) {
            setFormData({
                employeeID: timesheet.employeeID || "",
                projectID: timesheet.projectID || "",
                date: timesheet.date ? new Date(timesheet.date).toISOString().split("T")[0] : "", // Ensure correct date format
                hours: timesheet.hours ? parseInt(timesheet.hours, 10) : 0
            });
        } else {
            setFormData({ employeeID: "", projectID: "", date: "", hours: 0 }); // Reset form when adding new timesheet
        }
    }, [timesheet]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: name === "hours" ? Math.max(0, parseInt(value, 10) || 0) : value // Prevent negative hours
        }));
    };

    const handleSubmit = () => {
        if (!formData.employeeID || !formData.projectID || !formData.date || formData.hours === "") {
            alert("All fields are required!");
            return;
        }

        if (JSON.stringify(formData) === JSON.stringify(timesheet)) {
            alert("No changes made.");
            return;
        }

        handleSave(formData);
    };

    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
            <DialogTitle sx={{ backgroundColor: "#1976d2", color: "#fff", textAlign: "center" }}>
                {timesheet ? "Edit Timesheet" : "Add Timesheet"}
            </DialogTitle>
            <DialogContent sx={{ padding: "20px" }}>
                <TextField 
                    label="Employee ID" 
                    name="employeeID" 
                    fullWidth 
                    margin="dense" 
                    value={formData.employeeID} 
                    onChange={handleChange} 
                    required 
                />
                <TextField 
                    label="Project ID" 
                    name="projectID" 
                    fullWidth 
                    margin="dense" 
                    value={formData.projectID} 
                    onChange={handleChange} 
                    required 
                />
                <TextField 
                    label="Date" 
                    name="date" 
                    type="date" 
                    fullWidth 
                    margin="dense" 
                    value={formData.date} 
                    onChange={handleChange} 
                    required 
                />
                <TextField 
                    label="Hours" 
                    name="hours" 
                    type="number" 
                    fullWidth 
                    margin="dense" 
                    value={formData.hours} 
                    onChange={handleChange} 
                    required 
                    inputProps={{ min: 0 }} // Prevents negative values
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose} color="secondary" variant="outlined">Cancel</Button>
                <Button onClick={handleSubmit} color="primary" variant="contained">Save</Button>
            </DialogActions>
        </Dialog>
    );
};

// ✅ **PropTypes for Validation**
TimesheetForm.propTypes = {
    open: PropTypes.bool.isRequired,
    handleClose: PropTypes.func.isRequired,
    handleSave: PropTypes.func.isRequired,
    timesheet: PropTypes.shape({
        employeeID: PropTypes.string,
        projectID: PropTypes.string,
        date: PropTypes.string,
        hours: PropTypes.oneOfType([PropTypes.string, PropTypes.number])
    })
};

// ✅ **Default Props to Prevent Errors**
TimesheetForm.defaultProps = {
    timesheet: null
};

export default TimesheetForm;
