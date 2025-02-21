import PropTypes from "prop-types"; 
import { Table, TableHead, TableRow, TableCell, TableBody, IconButton, Paper, TableContainer } from "@mui/material";
import { Edit, Delete } from "@mui/icons-material";

const TimesheetTable = ({ timesheets, onEdit, onDelete }) => {
    return (
        <TableContainer component={Paper} sx={{ maxWidth: "90%", margin: "20px auto", boxShadow: 3, borderRadius: 2 }}>
            <Table sx={{ minWidth: 650 }}>
                <TableHead sx={{ backgroundColor: "#1976d2" }}> 
                    <TableRow>
                        <TableCell sx={headerCellStyle}>Employee ID</TableCell>
                        <TableCell sx={headerCellStyle}>Project ID</TableCell>
                        <TableCell sx={headerCellStyle}>Date</TableCell>
                        <TableCell sx={headerCellStyle}>Hours</TableCell>
                        <TableCell sx={headerCellStyle}>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {timesheets.map((row) => (
                        <TableRow key={row.id} sx={rowStyle}>
                            <TableCell>{row.employeeID}</TableCell>
                            <TableCell>{row.projectID}</TableCell>
                            <TableCell>{new Date(row.date).toLocaleDateString()}</TableCell>
                            <TableCell>{row.hours}</TableCell>
                            <TableCell>
                                <IconButton onClick={() => onEdit(row)} color="primary">
                                    <Edit />
                                </IconButton>
                                <IconButton onClick={() => onDelete(row.id)} color="error">
                                    <Delete />
                                </IconButton>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

// ✅ Styles using Material-UI's sx prop
const headerCellStyle = {
    color: "#fff",
    fontWeight: "bold",
    textAlign: "center"
};

const rowStyle = {
    "&:nth-of-type(odd)": { backgroundColor: "#f5f5f5" }, // Alternate row colors
    "&:hover": { backgroundColor: "#e3f2fd" }, // Row hover effect
    transition: "background 0.3s"
};

// ✅ Define PropTypes for validation
TimesheetTable.propTypes = {
    timesheets: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
            employeeID: PropTypes.string.isRequired,
            projectID: PropTypes.string.isRequired,
            date: PropTypes.string.isRequired,
            hours: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired
        })
    ).isRequired,
    onEdit: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired
};

export default TimesheetTable;
