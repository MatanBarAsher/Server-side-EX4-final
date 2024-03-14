$(document).ready(() => {
  server = `https://proj.ruppin.ac.il/cgroup52/test2/tar1/`;
  // let port = 7294;
  // server = `https://localhost:${port}/`;
});

const btnLogin = document.querySelector("#btnLogin");
const reigsterForm = document.querySelector("#register-form");
const btnCloseModal = document.querySelector("#btnModalClose");
const modal = document.getElementById("loginModal");
const modalForm = document.querySelector("#modal-form");
let tempUser = "";

btnLogin.addEventListener("click", handleLoginClick);
btnCloseModal.addEventListener("click", (e) => {
  e.preventDefault();
  modal.close();
});
modalForm.addEventListener("submit", handleModalSubmit);

reigsterForm.addEventListener("submit", handleRegistrationFormSubmit);

function handleLoginClick() {
  modal.showModal();
}

function handleModalSubmit(e) {
  e.preventDefault();
  const email = document.querySelector("#loginEmail").value;
  const password = document.querySelector("#loginPassword").value;
  user = {
    FirstName: "",
    FamilyName: "",
    Email: email,
    Password: password,
  };
  let api = server + `api/Users/Login`;
  ajaxCall("POST", api, JSON.stringify(user), LoginSCB, LoginECB);
}

function LoginSCB(obj) {
  if (obj) {
    console.log(obj);
    modal.close();
    if (obj.isActive) {
      setUserAsLogedIn(obj);
      swal({
        title: "Welcome!",
        text: "You are logged in!",
        icon: "success",
      }).then(function () {
        if (obj.isAdmin) {
          location.href = "./admin.html";
        } else {
          location.href = "./flats.html";
        }
      });
    } else {
      swal({
        title: " Oops!",
        text: "Your user is not active",
        icon: "error",
        button: "oh no!",
      });
    }
  } else {
    LoginECB(); // Call error handler for login failure
  }
}

function LoginECB() {
  console.log("Login failed");
  modal.close();
  swal({
    title: "Oops!",
    text: "Invalid email or password. Please try again.",
    icon: "error",
    button: "OK",
  });
}

function handleRegistrationFormSubmit(e) {
  e.preventDefault();
  const user = {
    firstName: $("#inpFirstName").val(),
    familyName: $("#inpFamilyName").val(),
    email: $("#inpEmail").val(),
    password: $("#inpPassword").val(),
    isActive: true,
  };
  tempUser = user;
  let api = server + `api/Users`;
  ajaxCall("POST", api, JSON.stringify(user), postSCB, postECB);
}

function postSCB(obj) {
  setUserAsLogedIn(tempUser);

  swal({ title: "Welcome!", text: "You are registerd!", icon: "success" }).then(
    function () {
      location.href = "./flats.html";
    }
  );
}

function postECB(obj) {
  tempUser = "";
  console.log(obj);
  swal({
    title: " Oops!",
    text: " This email is already taken!",
    icon: "error",
    button: "oh no!",
  });
}

function setUserAsLogedIn(user) {
  sessionStorage.setItem("current-user", JSON.stringify(user));
}
