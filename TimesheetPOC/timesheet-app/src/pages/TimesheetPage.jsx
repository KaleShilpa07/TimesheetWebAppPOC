import { useState, useEffect } from "react";
import { getTimesheets, addTimesheet, updateTimesheet, deleteTimesheet } from "../services/api";
import TimesheetTable from "../components/TimesheetTable";
import TimesheetForm from "../components/TimesheetForm";
import { Button, Container, Box, Typography } from "@mui/material";

const TimesheetPage = () => {
    const [timesheets, setTimesheets] = useState([]);
    const [selectedTimesheet, setSelectedTimesheet] = useState(null);
    const [open, setOpen] = useState(false);

    useEffect(() => {
        fetchTimesheets();
    }, []);

    const fetchTimesheets = async () => {
        const data = await getTimesheets();
        setTimesheets(data);
    };

    const handleSave = async (timesheet) => {
        if (selectedTimesheet) {
            await updateTimesheet(selectedTimesheet.id, timesheet);
        } else {
            await addTimesheet(timesheet);
        }
        fetchTimesheets();
        setOpen(false);
        setSelectedTimesheet(null);
    };

    const handleDelete = async (id) => {
        await deleteTimesheet(id);
        fetchTimesheets();
    };

    return (
        <Container maxWidth="md">
            {/* Header & Button Section */}
            <Box display="flex" justifyContent="space-between" alignItems="center" mt={4} mb={2}>
                <Typography variant="h4" fontWeight="bold">
                    Timesheet Management
                </Typography>
                <Button 
                    variant="contained" 
                    color="primary" 
                    onClick={() => { setSelectedTimesheet(null); setOpen(true); }} // Reset form on Add
                    sx={{ fontSize: "1rem", px: 3, py: 1 }}
                >
                    Add Timesheet
                </Button>
            </Box>

            {/* Table or No Data Message */}
            {timesheets.length > 0 ? (
                <TimesheetTable 
                    timesheets={timesheets} 
                    onEdit={(ts) => { setSelectedTimesheet(ts); setOpen(true); }} 
                    onDelete={handleDelete} 
                />
            ) : (
                <Typography variant="h6" color="textSecondary" align="center" mt={4}>
                    No timesheets available. Click Add Timesheet to create one.
                </Typography>
            )}

            {/* Timesheet Form Dialog */}
            <TimesheetForm 
                open={open} 
                handleClose={() => setOpen(false)} 
                handleSave={handleSave} 
                timesheet={selectedTimesheet} 
            />
        </Container>
    );
};

export default TimesheetPage;
