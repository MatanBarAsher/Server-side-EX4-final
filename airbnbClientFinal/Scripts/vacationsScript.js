// get the relevant flat id from local storage
const myFlatId = sessionStorage.getItem("flatId");
let myVacations;

$(document).ready(() => {
  const myUserEmail = currentUser.email;
  server = `https://proj.ruppin.ac.il/cgroup52/test2/tar1/`;
  // let port = 7294;
  // server = `https://localhost:${port}/`;
  setFlatId(myFlatId);
  setUserEmail(myUserEmail);
  $("#postForm").submit(checkDates);
  $("#btnShowVacations").click(getVacations);
});

function getVacations() {
  let api = server + "api/Orders";
  ajaxCall("GET", api, "", getSCB, getECB);
}
function getSCB(obj) {
  console.log(obj);
  if (obj) {
    myVacations = obj;
    renderVacations(obj);
  } else {
    swal({
      title: "Oops!",
      text: "No vacations to show.",
      icon: "error",
      button: "OK",
    });
  }
}
function getECB(obj) {
  console.log("error");
}

function submitVacation() {
  let vacation = {
    userEmail: $("#inpUserEmail").val(),
    flatId: $("#inpFlatId").val(),
    startDate: $("#inpStartDate").val(),
    endDate: $("#inpEndDate").val(),
  };
  let api = server + "api/Orders";
  ajaxCall("POST", api, JSON.stringify(vacation), postSCB, postECB);
  return false;
}

function postSCB(obj) {
  console.log(obj);
  confetti = document.getElementById("confetti");
  if (obj == 1) {
    confetti.style.display = "block";
    swal({
      title: "Hopa!",
      text: "New vacation created!",
      icon: "success",
    }).then(function () {
      confetti.style.display = "none";
    });
  } else {
    swal({
      title: "Oops!",
      text: "check your order details",
      icon: "error",
      button: "OK",
    });
  }
}

function postECB(obj) {
  console.log(obj);
  swal({
    title: " Oops!",
    text: " Seems like these date is taken!",
    icon: "error",
    button: "oh no!",
  });
}

function renderVacations(vacations) {
  let myUserEmail = $("#inpUserEmail").val();
  let str = "No vacations to show.";
  if (vacations.length) {
    if (myUserEmail) {
      str = "";
      Object.values(vacations).forEach((v) => {
        if (v.userEmail === myUserEmail) {
          let template = `
          <div class='card' style="margin:10px"><h2>🍸</h2>
          <p><b>User email:</b> ${v.userEmail}</p>
          <p><b>Flat ID:</b> ${v.flatId}</p>
          <p><b>Start date:</b> ${v.startDate.slice(0, 10)}</p>
          <p><b>End date:</b> ${v.endDate.slice(0, 10)}</p>
          </div>`;
          str += template;
        }
      });
    }
  }
  $("#view_container").html(str);
}

function setFlatId(id) {
  $("#inpFlatId").prop("value", id);
  $("#inpFlatId").attr("readonly", true);
}

function setUserEmail(email) {
  $("#inpUserEmail").prop("value", email);
  $("#inpUserEmail").attr("readonly", true);
}

function checkDates() {
  let start = new Date($("#inpStartDate").val());
  let end = new Date($("#inpEndDate").val());

  // Check if start date is in the past
  let today = new Date();
  today.setHours(0, 0, 0, 0); // Set time to midnight for accurate comparison
  if (start < today) {
    swal({
      title: "Oops!",
      text: "Start date cannot be in the past.",
      icon: "error",
      button: "OK",
    });
    return false;
  }

  let diffTime = Math.round(
    (end.getTime() - start.getTime()) / (1000 * 3600 * 24)
  );
  if (diffTime <= 0) {
    swal({
      title: "Oops!",
      text: "End date must be after start date.",
      icon: "error",
      button: "OK",
    });
    return false;
  }
  if (diffTime > 20) {
    swal({
      title: "Oops!",
      text: "The vacation can't be more than 20 days.",
      icon: "error",
      button: "OK",
    });
    return false;
  }
  submitVacation();
  return false;
}
