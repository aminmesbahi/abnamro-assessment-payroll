// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const actionsTd = `<div class="rowActions" style="text-align:right">
            <button onclick="downloadAllPayments(this)" class="btn download_all"><i class="bi bi-file-earmark-arrow-down" data-bs-toggle="tooltip" title="Download Salary History as CSV file"></i></button>
            <button onclick="initPaymentHistoryModal(this)" class="btn contracts"  data-bs-toggle="modal" data-bs-target="#employeeHistoryModal"><i class="bi bi-clock-history" data-bs-toggle="tooltip" title="History"></i></button>
            <button id="openEditEmployeeModalButton" onclick="editEmployee(this)" class="btn edit" data-bs-toggle="modal" data-bs-target="#editEmployeeModal"><i class="bi bi-pencil" data-bs-toggle="tooltip" title="Edit"></i></button>
            <button onclick="deleteEmployee(this)" class="btn delete"  data-bs-toggle="modal" data-bs-target="#deleteEmployeeModal"><i class="bi bi-trash" data-bs-toggle="tooltip" title="Delete"></i></button>
            </div>`;
const baseApiUrl ="https://localhost:7124/";
var employeesTable;

$(document).ready(function () {
    // Initialize Employees Table
        initEmployeesTable();
        $('#addEmployeeForm input').on('focusout focus change', function () {        
        if (!this.checkValidity()) {
            $(this).removeClass('is-valid is-invalid').addClass('is-invalid');
        }
                else {
                $(this).removeClass('is-valid is-invalid').addClass('is-valid');
            }
    });
        $('#editEmployeeForm input').on('focusout focus change', function () {
        if (!this.checkValidity()) {
            $(this).removeClass('is-valid is-invalid').addClass('is-invalid');
        }
        else {
            $(this).removeClass('is-valid is-invalid').addClass('is-valid');
        }
    });

    // Add New Employee
        $("#addEmployeeButton").on("click", function (event) {
        $('#addEmployeeForm').removeClass('is-valid is-invalid');
        if ($('#addEmployeeForm.needs-validation')[0].checkValidity() === false) {
            $("#addEmployeeForm input").each(function () {
                if (!this.checkValidity()) {
                    $(this).removeClass('is-valid is-invalid').addClass('is-invalid');
                }
            });
        } else {
            let newEmployee = $("#addEmployeeForm").serializeJSON();
            newEmployee.birthDate = new Date(newEmployee.birthDate).toISOString();
            $.ajax({
                url: baseApiUrl + "api/employees",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(newEmployee),
                success: function (result) {
                    $("#newEmployeeSuccessAlert").fadeIn(1000);
                    setTimeout(function () {
                        $("#newEmployeeSuccessAlert").fadeOut(1000);
                    }, 2000); setTimeout(function () {
                        $("#addEmployeeModal").modal("hide");
                        $(":input", "#addEmployeeForm")
                            .not(":button, :submit, :reset, :hidden")
                            .val("")
                            .prop("checked", false)
                            .prop("selected", false);
                        $("#employeesTable").DataTable().ajax.reload();
                    }, 3000);
                },
                error: function (xhr, resp, text) {
                    console.log(xhr, resp, text);
                }
            });
        }
        });
    
    // Edit Employee
        $('#editEmployeeButton').on("click", function (event) {
        event.preventDefault();
            $('#editEmployeeForm').removeClass('is-valid is-invalid');
            if ($('#editEmployeeForm.needs-validation')[0].checkValidity() === false) {
                $("#editEmployeeForm input").each(function () {
                    if (!this.checkValidity()) {
                        $(this).removeClass('is-valid is-invalid').addClass('is-invalid');
                    }
                });
            } else {
                let editedEmployee = $("#editEmployeeForm").serializeJSON();
                editedEmployee.id = parseInt($("#item_id").val());
                editedEmployee.birthDate = new Date(editedEmployee.birthDate).toISOString();
                $.ajax({
                    url: baseApiUrl + "api/employees",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(editedEmployee),
                    type: "PUT",
                    success: function (response) {
                        $("#editEmployeeSuccessAlert").fadeIn(1000);
                        setTimeout(function () {
                            $("#editEmployeeSuccessAlert").fadeOut(1000); $("#editEmployeeModal").modal("hide");
                        }, 2000); setTimeout(function () {
                            $("#addEmployeeModal").modal("hide");
                            $(":input", "#addEmployeeForm")
                                .not(":button, :submit, :reset, :hidden")
                                .val("")
                                .prop("checked", false)
                                .prop("selected", false);
                        });
                        employeesTable.ajax.reload(null, false);
                    }
                })
            }
        });

    // Delete Employee
        $('#deleteEmployeeButton').on('click', function (e) {
          e.preventDefault();
          var deletingEmployee = {};
          deletingEmployee.id = parseInt($("#item_id").val());
          $.ajax({
            url: baseApiUrl+"api/employees/" + deletingEmployee.id,
            contentType: "application/json",
            dataType: "json",        
            type: "DELETE",
            success: function (response) {
              $("#deleteEmployeeSuccessAlert").fadeIn(1000);
              setTimeout(function () {
                $("#deleteEmployeeSuccessAlert").fadeOut(1000); $("#deleteEmployeeModal").modal("hide");
              }, 2000);
    
              employeesTable.ajax.reload(null, false);
            }
          })
        });
    });

function initEmployeesTable() {
    employeesTable = $('#employeesTable').DataTable({
        "ajax": {
          "url": baseApiUrl+"api/employees?PageSize=10000&PageIndex=0",
          "dataSrc": "employees"
        },
        "columns": [
          { "data": "firstName" },
          { "data": "lastName" },
            { "data": "age" },
            { "data": null, "defaultContent": actionsTd }
        ],
        rowCallback: function (row, data, index) {
            if (data.id >4) {
                $("td:eq(3)", row).find(".contracts").addClass('d-none');
                $("td:eq(3)", row).find(".download_all").addClass('d-none');
            }
        }
      });
      return employeesTable;
}

function deleteEmployee(this_el) {
    var tr_el = this_el.closest("tr");
    var row = employeesTable.row(tr_el);
    var row_data = row.data();
    $("#item_id").val(row_data.id);
}

function editEmployee(this_el) {
    var tr_el = this_el.closest("tr");
    var row = employeesTable.row(tr_el);
    var row_data = row.data();
    $("#item_id").val(row_data.id);
    $("#editEmployeeModal #firstName").val(row_data.firstName);
    $("#editEmployeeModal #lastName").val(row_data.lastName);
    let date = row_data.birthDate;
    $("#editEmployeeModal #birthDate").val(date.substring(0, 10));
}

function downloadPaymentHistory(paymentHistoryId) {
    $.fileDownload(baseApiUrl+"api/employees/salary-report/" + paymentHistoryId)
      .done(function () { alert('File download a success!'); })
      .fail(function () { alert('File download failed!'); });
}

function downloadAllPayments(this_el) {
    $("#item_id").val(this_el);
    var tr_el = this_el.closest("tr");
    var row = employeesTable.row(tr_el);
    var row_data = row.data();
    $("#item_id").val(row_data.id);
    $.fileDownload(baseApiUrl+"api/employees/salary-report/all/" + row_data.id)
      .done(function () { console.log("File download a success!"); })
      .fail(function () { console.log("File download failed!"); });
}

function initPaymentHistoryModal (this_el) {    
    let contractsData;
    var tr_el = this_el.closest("tr");
    var row = employeesTable.row(tr_el);
    var row_data = row.data();
    $("#item_id").val(row_data.id);
    $.when(
      $.getJSON(baseApiUrl + "api/employees/history/" + row_data.id)
    ).done(function (json) {
        contractsData = json;
        $("#employeeContractsAccordion").html(
            $("#contractsTmpl").render(contractsData.Employee.Contracts)
      );
    });
}