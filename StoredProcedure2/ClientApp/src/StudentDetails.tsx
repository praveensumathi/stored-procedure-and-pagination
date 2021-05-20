import React, { useEffect, useState } from "react";
import { DefaultButton, PrimaryButton } from "@fluentui/react/lib/Button";
import Pagination from "@material-ui/lab/Pagination";

interface IStudentDetails {
  id: number;
  firstName: string;
  lastName: string;
  age: number;
}

function StudentDetails() {
  const [studentDetails, setStudentDetails] = useState<IStudentDetails[]>([]);
  const [pageNumber, setPageNumber] = useState(1);

  useEffect(() => {
    fetchStudentDetails(pageNumber);
  }, [pageNumber]);

  return (
    <div>
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>ID</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Age</th>
          </tr>
        </thead>
        <tbody>
          {studentDetails.map((student) => (
            <tr key={student.id}>
              <td>{student.id}</td>
              <td>{student.firstName}</td>
              <td>{student.lastName}</td>
              <td>{student.age}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <Pagination
        count={10}
        page={pageNumber}
        color="secondary"
        onChange={handleChange}
      />
    </div>
  );

  function handleChange(event, value) {
    setPageNumber(value);
  }
  async function fetchStudentDetails(pageNumber: number) {
    const response = await fetch(`/student?pageNumber=${pageNumber}`);
    const data: IStudentDetails[] = await response.json();
    setStudentDetails(data);
  }
}

export default StudentDetails;
