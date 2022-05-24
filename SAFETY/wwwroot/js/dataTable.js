
var DTableOption = {
    "sSearch": "搜尋",
    oPaginate: {
        sPrevious: " < ",
        sNext: " > "
    },
    "sLengthMenu": localizer_show+' <select  style="margin-bottom:5px;">' +
        '<option value="10">10</option>' +
        '<option value="20">20</option>' +
        '<option value="30">30</option>' +
        '<option value="40">40</option>' +
        '<option value="50">50</option>' +
        '<option value="-1">' + localizer_all+'</option>' +
        '</select> ' + localizer_row,
    "sInfo": localizer_show + " _START_ " + localizer_to + " _END_ "  + localizer_data + " (" + localizer_total + " _TOTAL_ "  + localizer_data + ")",
    "sEmptyTable": "<div class='no-data-card'>查無資料</div>"
}

var firstRun = [];
var refreshCall;
var excel_result;
var excel_col;
var selected = [];

function LoadDataTable(dataUrl, queryData, columnSet, columnDef, issearch, editable, urls, refreshCallBack, customDtId, reuse) {
    if (reuse == null) {
        reuse = true;
    }

    //createLoader();

    var dtId = customDtId == null ? "dataTable" : customDtId;

    refreshCall = refreshCallBack;

    var result = null;
    return $.ajax(
        {
            type: 'POST',
            url: dataUrl,
            data: JSON.stringify(queryData),
            dataType: 'json',
            contentType: 'application/json',
            async: true,
            success: function (e) {
                result = e.dt;
                excel_result = result;
                excel_col = columnSet;
            },
            complete: function (e) {
                //AjaxHandler(e);

                LoadData(result, columnSet, columnDef, issearch, editable, urls, dtId, reuse);
                //removeLoader();
            }
        });
}


function LoadData(result, columnSet, columnDef, issearch, editable, urls, dtId, reuse) {
    var dataTableId = "#" + dtId;

    if (result.length == 0) {
        var d = new Date().toISOString().slice(0, 10);

        $("#EmptyAlert").html("<h2>資料庫查無資料 (" + d + ")</h2>");
        $("#EmptyAlert").show();
        //$("#" + dtId + "_wrapper").hide();
    } else {
        $("#EmptyAlert").hide();
        $("#" + dtId + "_wrapper").show();
    }

    if (result != null ) {
        if (firstRun.indexOf(dataTableId) >= 0 && reuse) {
            if (result.length >= 0) {
                $(dataTableId).DataTable().clear().draw();
                $(dataTableId).DataTable().rows.add(result); // Add new data
                $(dataTableId).DataTable().columns.adjust().draw();
            } 
        } else {
            if (!reuse) {
                if (firstRun.indexOf(dataTableId) >= 0) {
                    $(dataTableId).DataTable().destroy();
                    $(dataTableId).html('');
                }
            }
            var origin = {
                data: result,
                columns: columnSet,
                columnDefs: columnDef,
                "oLanguage": DTableOption,
                searching: issearch,
                //destroy:true,
                order: false,
                info: true,
                dom: 'Blfrtip',//B代表 buttons、l代表 sLengthMenu
                buttons: [
                    {
                        extend: 'excelHtml5',
                        title: titleBag,
                        text: "匯出Excel",
                        customize: function (xlsx) {
                        }
                    }
                ],
                select: true,
            };
            firstRun.push(dataTableId);

            var editor = {
                //dom: 'Bfrtip',        // Needs button container
                select: 'single',
                responsive: true,
                altEditor: true,     // Enable altEditor
                buttons: [
                    {
                        text: '新增',
                        name: 'add',        // do not change name
                        attr: {
                            id: 'copyButton'
                        }
                    },
                    {
                        extend: 'selected', // Bind to Selected row
                        text: '編輯',
                        name: 'edit'        // do not change name
                    },
                    {
                        extend: 'selected', // Bind to Selected row
                        text: '刪除',
                        name: 'delete'      // do not change name
                    },
                    {
                        text: '重查',
                        name: 'refresh'      // do not change name
                    }
                ],
                onAddRow: function (datatable, rowdata, success, error) {
                    $.ajax({
                        // a tipycal url would be / with type='PUT'
                        url: urls['create'],
                        type: 'GET',
                        data: rowdata,
                        success: success,
                        error: error,
                        complete: function (e) {
                            //AjaxHandler(e);
                        }
                    });
                },
                onDeleteRow: function (datatable, rowdata, success, error) {
                    $.ajax({
                        // a tipycal url would be /{id} with type='DELETE'
                        url: urls['delete'],
                        type: 'GET',
                        data: rowdata,
                        success: success,
                        error: error,
                        complete: function (e) {
                            //AjaxHandler(e);
                        }
                    });
                },
                onEditRow: function (datatable, rowdata, success, error) {
                    $.ajax({
                        // a tipycal url would be /{id} with type='POST'
                        url: urls['update'],
                        type: 'GET',
                        data: rowdata,
                        success: success,
                        error: error,
                        complete: function (e) {
                            //AjaxHandler(e);
                        }
                    });
                }
            };

            if (editable) {
                if (typeof Object.assign != 'function') {
                    Object.assign = function (target) {
                        'use strict';
                        if (target == null) {
                            throw new TypeError('Cannot convert undefined or null to object');
                        }

                        target = Object(target);
                        for (var index = 1; index < arguments.length; index++) {
                            var source = arguments[index];
                            if (source != null) {
                                for (var key in source) {
                                    if (Object.prototype.hasOwnProperty.call(source, key)) {
                                        target[key] = source[key];
                                    }
                                }
                            }
                        }
                        return target;
                    };
                }

                Object.assign(origin, editor);
            }

            $(dataTableId).DataTable(origin);
        }
    } else {
        if (firstRun.indexOf(dataTableId) >= 0) {
            $(dataTableId).DataTable().clear().draw();
            $(dataTableId).DataTable().rows.add(result); // Add new data
            $(dataTableId).DataTable().columns.adjust().draw();
        }
    }
}



function dataTable_save(surl, rowdata) {
    //createLoader();


    var res = false;

    $.ajax({
        // a tipycal url would be /{id} with type='POST'
        url: surl,
        type: 'GET',
        data: rowdata,
        async: false,
        success: function (resultdata) {
            //$(dataTableId).DataTable().ajax.reload();
            res = true;
            //removeLoader();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.statusText);
        },
        complete: function (e) {
            //AjaxHandler(e);
        }
    });

    return res;
}

