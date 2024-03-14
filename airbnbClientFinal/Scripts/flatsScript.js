$(document).ready(() => {
  server = `https://proj.ruppin.ac.il/cgroup52/test2/tar1/`;
  // let port = 7294;
  // server = `https://localhost:${port}/`;
  $("#postForm").submit(submitFlat);
  getFlats();
});

function getFlats() {
  let api = server + "api/Flats";
  ajaxCall("GET", api, "", getSCB, getECB);
}
function getSCB(obj) {
  console.log(obj);
  renderFlats(obj);
}
function getECB(obj) {
  console.log(obj);
}

function submitFlat() {
  let flat = {
    Id: $("#inpId").val(),
    City: $("#inpCity").val(),
    Address: $("#inpAddress").val(),
    NumOfRooms: parseFloat($("#inpNumOfRooms").val()),
    Price: parseFloat($("#inpPrice").val()),
  };
  let api = server + "api/Flats";
  ajaxCall("POST", api, JSON.stringify(flat), postSCB, postECB);
  return false;
}

function postSCB(obj) {
  console.log(obj);
  if (obj == 1) {
    swal("We made it!", "Flat added!", "success");
    getFlats();
  } else {
    swal({
      title: " Oops!",
      text: " This flat already exist!",
      icon: "error",
      button: "oh no!",
    });
  }
}

function postECB(obj) {
  console.log(obj);
  swal({
    title: " Oops!",
    text: " This flat already exist!",
    icon: "error",
    button: "oh no!",
  });
}

function renderFlats(flats) {
  let str = "";
  console.log(typeof flats);
  Object.values(flats).forEach((f) => {
    let temp = "<div class='card'><h2>🏠</h2>";
    for (key in f) {
      temp += `<p><b>${key}:</b> ${f[key]}</p>`;
    }
    temp += `<input type="button" value="Order now" name="" id="${f.id}BTN" onclick='newOrder("${f.id}")'/></div>`;
    str += temp;
  });
  $("#view_container").html(str);
}

function newOrder(id) {
  sessionStorage.setItem("flatId", id);
  location.href = "./vacations.html";
}
