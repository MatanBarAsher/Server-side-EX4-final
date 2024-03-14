let currentUser = JSON.parse(sessionStorage.getItem("current-user"));
let tempUser;
function loadNavBar() {
  let nav = document.createElement("nav");
  nav.id = "main-nav";
  nav.innerHTML = `
  <div id='user-nav'><p><b>wellcome, </b>${currentUser.firstName}</p></div>
  <div id='buttons'>
  <button id="btnFlats">Flats</button>
  <button id="btnEdit">Edit details</button>
  <button id="btnLogOut">Log out</button>
  <dialog id="modalEdit"></dialog>
  </div>
  `;
  document.body.insertAdjacentElement("afterbegin", nav);
}
loadNavBar();

const modalEdit = document.getElementById("modalEdit");

document.getElementById("btnFlats").addEventListener("click", () => {
  location.href = "./flats.html";
});

document.getElementById("btnEdit").addEventListener("click", handleEditClick);
document.getElementById("btnLogOut").addEventListener("click", () => {
  currentUser = "";
  sessionStorage.setItem("current-user", "");
  location.href = "./index.html";
});

function handleEditClick() {
  const template = `
  <form id="edit-form">
        <table>
          <tbody>
            <tr>
              <td>First name:</td>
              <td>
                <input
                  type="text"
                  name="firstName"
                  id="inpFirstName"
                  defaultValue='${currentUser.firstName}'
                  value='${currentUser.firstName}'
                  required
                />
              </td>
            </tr>
            <tr>
              <td>Family name:</td>
              <td>
                <input
                  type="text"
                  name="familyName"
                  id="inpFamilyName"
                  defaultValue='${currentUser.familyName}'
                  value='${currentUser.familyName}'
                  />
                  </td>
                  </tr>
                  <tr>
                  <td>Email:</td>
                  <td><input type="email" required id="inpEmail" defaultValue='${currentUser.email}' value='${currentUser.email}' readonly /></td>
            </tr>
            <tr>
              <td>Password:</td>
              <td>
                <input
                  type="password"
                  name="password"
                  id="inpPassword"
                  defaultValue='${currentUser.password}'
                  value='${currentUser.password}'
                  required
                />
              </td>
            </tr>
          </tbody>
        </table>
        <button type="submit">Edit</button>
      </form>
      <button id="btnModalClose">Close</button>
  `;

  modalEdit.innerHTML = "";
  modalEdit.insertAdjacentHTML("afterbegin", template);
  document.getElementById("btnModalClose").addEventListener("click", () => {
    modalEdit.close();
  });
  document
    .getElementById("edit-form")
    .addEventListener("submit", (e) => submitEdit(e));
  modalEdit.showModal();
}

function submitEdit(e) {
  e.preventDefault();
  tempUser = {
    firstName: $("#inpFirstName").val(),
    familyName: $("#inpFamilyName").val(),
    email: $("#inpEmail").val(),
    password: $("#inpPassword").val(),
    isActive: currentUser.isActive,
  };
  let api = server + `api/Users/Update`;
  ajaxCall("PUT", api, JSON.stringify(tempUser), EditSCB, EditECB);
}

function EditSCB() {
  modalEdit.close();
  sessionStorage.setItem("current-user", JSON.stringify(tempUser));
  loadNavBar();
  swal({
    title: "Your details have chaned, but we love you just the way you are.",
    text: "",
    icon: "success",
  }).then(function () {
    location.reload();
  });
}
function EditECB(err) {
  console.log(err);
}
