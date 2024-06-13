@model IEnumerable < _483449_jQuery_DataTable_RowUpdate_Modal_MVC.Customer >

    @{
        Layout = null;
    }

    < !DOCTYPE html >

        <html>
            <head>
                <meta name="viewport" content="width=device-width" />
                <title>Index</title>
                <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
                <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                <script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
                <link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />
                <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
                <link rel="stylesheet" media="screen" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' />
                <script>
                    var row;
                    $(function () {
                        $('#tblCustomers').DataTable();
                    });
                    function GetCustomer(ele, id) {
                        row = $(ele).closest('tr');
                    $.ajax({
                        url: "/Home/GetCustomer",
                    data: {Id: id },
                    type: "Get",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "Json",
                    success: function (result) {
                        $('#txtID').val(result.data.CustomerId);
                    $('#txtName').val(result.data.Name);
                    $('#txtCountry').val(result.data.Country);

                    $('#btnSave').hide();
                    $('#btnUpdate').show();
                    $('#CustomerModal .close').css('display', 'none');
                    $('#CustomerModal').modal('show');
                }
            })
        }

                    function UpdateCustomer() {
                    var table = $('#tblCustomers').DataTable();
                    table.destroy();
                    var model = { };
                    model.CustomerId = $('#txtID').val();
                    model.Name = $('#txtName').val();
                    model.Country = $('#txtCountry').val();

                    $.ajax({
                        url: "/Home/UpdateCustomer",
                    type: "Post",
                    data: JSON.stringify(model),
                    contentType: "application/json; charset=UTF-8",
                    dataType: "json",
                    success: function (response) {
                        $('#CustomerModal .close').css('display', 'none');
                    $('#CustomerModal').modal('hide');
                    table.cell(row, 1).data($('#txtName').val());
                    table.cell(row, 2).data($('#txtCountry').val());
                    table.draw();
                }
            });

        }
                </script>
            </head>
            <body>
                <table id="tblCustomers" class="table table-responsive-sm table-condensed  table-bordered">
                    <thead>
                        <tr>
                            <th>@Html.DisplayNameFor(model => model.CustomerId)</th>
                            <th>@Html.DisplayNameFor(model => model.Name)</th>
                            <th>@Html.DisplayNameFor(model => model.Country)</th>
                            <th>Aksiyon</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var item in Model)
                        {
                            <tr>
                                <td>@item.CustomerId</td>
                                <td>@item.Name</td>
                                <td>@item.Country</td>
                                <td>
                                    <a class='fa fa-list' onclick="GetCustomer(this,@item.CustomerId)" style='color: #428bca;' data-toggle='tooltip' title='UpdateCustomer' href='javascript:;'></a>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
                <div class="modal fade" id="CustomerModal" tabindex="-1" role="dialog" aria-labelledby="myModalLable" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title" style="margin: 0 auto;" id="ModalTitle">Ana Menü Ekle</h4>
                            </div>
                            <div class="modal-body">
                                <form class="form-horizontal" id="mConta">
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <input id="txtID" style="display:none" type="text" placeholder="ID" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <input id="txtName" class="form-control tooltip-test" title="Name" type="text" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <input id="txtCountry" class="form-control tooltip-test" type="text" title="Country" />
                                        </div>
                                    </div>
                                </form>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" id="btnClose" data-dismiss="modal">Close</button>
                                <button type="button" class="btn btn-success" id="btnSave">
                                    Save <span class="fa fa-floppy-o" aria-hidden="true"></span>
                                </button>
                                <button type="button" class="btn btn-success" id="btnUpdate" onclick="UpdateCustomer()">
                                    Update <span class="fa fa-floppy-o" aria-hidden="true"></span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
        </html>