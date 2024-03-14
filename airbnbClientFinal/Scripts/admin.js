$(document).ready(() => {
  server = `https://proj.ruppin.ac.il/cgroup52/test2/tar1/`;
  // let port = 7294;
  // server = `https://localhost:${port}/`;
  addManageUsersBtn();

  const btnShowAverage = document.getElementById("btnShowAverage");
  const manageUsers = document.getElementById("btnManageUsers");

  manageUsers.addEventListener("click", getUsers);
  btnShowAverage.addEventListener("click", renderReport);
  $("#data").hide();
});
let tbl;

function getUsers() {
  let api = server + "api/Users";
  ajaxCall("GET", api, "", getSCB, getECB);
}

function getSCB(obj) {
  console.log(obj);
  renderUsers(obj);
}

function getECB(obj) {
  console.log(obj);
  swal("Error: " + obj);
}

function addManageUsersBtn() {
  const btn = `<button id="btnManageUsers">Manage users</button>`;
  document.getElementById("main-nav").insertAdjacentHTML("beforeend", btn);
}
function renderUsers(usersData) {
  users = usersData;
  try {
    if (tbl) {
      tbl.destroy();
    }
    tbl = $("#usersTable").DataTable({
      data: usersData,
      pageLength: 15,
      columns: [
        { data: "email" },
        { data: "firstName" },
        { data: "familyName" },
        { data: "password" },
        {
          render: function (data, type, row, meta) {
            if (row.firstName !== "admin") {
              let isActivCheckBox = `<input type='checkbox' class='isActive' id='${row.email}'`;

              if (row.isActive) {
                isActivCheckBox += " checked";
              }

              isActivCheckBox += "/>";
              return isActivCheckBox;
            } else {
              return "";
            }
          },
        },
      ],
    });
  } catch (err) {
    alert(err);
  }
  $(".isActive").on("change", (e) => changeIsActive(e, usersData));
  $("#data").show();
}

function changeIsActive(e, allUserData) {
  console.log(e);
  console.log(e.target);
  const myId = e.target.id;
  const currentUser = allUserData.find((user) => user.email === myId);

  if (currentUser) {
    currentUser.isActive = e.target.checked;
    let api = server + `api/Users/Update`;
    ajaxCall("PUT", api, JSON.stringify(currentUser), putSCB, putECB);
  } else {
    console.error("User not found");
  }
  renderUsers();
}

function putSCB() {
  getUsers();
}
function putECB(err) {
  console.log(err);
}

function renderReport() {
  const month = document.getElementById("monthsDropdown").value;
  let api = server + `api/Orders/Report/${month}`;
  ajaxCall("GET", api, "", reportSCB, reportECB);
}
function reportSCB(obj) {
  console.log(obj);
  let str = ``;
  obj.map((key) => {
    str += `<p>${key.city}: ${key.averagePrice}$ </p>`;
  });
  document.getElementById("results").innerHTML = str;
}
function reportECB(obj) {
  console.log(obj);
}
