// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var loadFile = function (event) {
    var output = document.getElementById('output');
    output.src = URL.createObjectURL(event.target.files[0]);
    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
};
function daysIntoYear(date) {
    return (Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()) - Date.UTC(date.getFullYear(), 0, 0)) / 24 / 60 / 60 / 1000;
}

// Получение всех пользователей
async function getUsers() {
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/users", {
        method: "GET",
        headers: {
            "Accept": "application/json", "Content-Type": "application/json; charset=utf-8",
            dataType: 'json' }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const users = await response.json();
        const rows = document.querySelector("tbody");
        // добавляем полученные элементы в таблицу
        var now = daysIntoYear(new Date());
        users.forEach((user, index, array) => {
            if (user.dayOfYear < now) { user.dayOfYear += 366 }
        });
        users.sort(function (a, b) {
            if (a.dayOfYear > b.dayOfYear) return 1; // если первое значение больше второго
            if (a.dayOfYear == b.dayOfYear) return 0; // если равны
            if (a.dayOfYear < b.dayOfYear) return -1; // если первое значение меньше второго
        });
        //console.log(users);

        var paramsString = document.location.search; // ?page=4&limit=10&sortby=desc  
        var searchParams = new URLSearchParams(paramsString);
        var edituserid = -1;

        if (searchParams.has("id")) {
            edituserid = searchParams.get("id");
        }
        searchParams.get("page"); // 4        
        users.forEach(user => rows.append(row(user, edituserid)));
    }
}
// Получение одного пользователя
async function getUser(id) {
    const response = await fetch(`/api/users/${id}`, {
        method: "GET",
        headers: {
            "Accept": "application/json", "Content-Type": "application/json; charset=utf-8",
            dataType: 'json' }
    });
    if (response.ok === true) {
        const user = await response.json();
        document.getElementById("userId").value = user.id;
        document.getElementById("userName").value = user.name;
        document.getElementById("userBirthday").value = user.birthday.slice(0, 10);
        document.getElementById("userType").checked = user.type;
        document.getElementById("output").src = user.photo;
    }
    else {
        // если произошла ошибка, получаем сообщение об ошибке
        const error = await response.json();
        console.log(error.message); // и выводим его на консоль
    }
}
// Добавление пользователя
async function createUser(userName, userBirthday, userType, userPhoto) {

    const response = await fetch("/api/users", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json; charset=utf-8", dataType: 'json' },
        body: JSON.stringify({
            name: userName,
            birthday: userBirthday,
            type: userType,
            photo: userPhoto,
        })
    });
    if (response.ok === true) {
        const user = await response.json();
        document.querySelector("tbody").append(row(user, -1));
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
// Изменение пользователя
async function editUser(userId, userName, userBirthday, userType, userPhoto) {
    const response = await fetch(`/api/users/${userId}`, {
            method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json; charset=utf-8", dataType: 'json' },
        body: JSON.stringify({
            id: userId,
            name: userName,
            birthday: userBirthday,
            type: userType,
            photo: userPhoto,
        })
            });
        if (response.ok === true) {
            const user = await response.json();
            document.querySelector(`tr[data-rowid='${user.id}']`).replaceWith(row(user));
        }
        else {
            const error = await response.json();
            console.log(error.message);
        }
}
// Удаление пользователя
async function deleteUser(id) {
    const response = await fetch(`/api/users/${id}/0`, {
        method: "DELETE",
        headers: { "Accept": "application/json", "Content-Type": "application/json; charset=utf-8", dataType: 'json' }
    });
    if (response.ok === true) {
        const user = await response.json();
        document.querySelector(`tr[data-rowid='${user.id}']`).remove();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

// сброс данных формы после отправки
function reset() {
    //location.reload();
    document.getElementById("userId").value =
        document.getElementById("userName").value = "";
    document.getElementById("userBirthday").value = "2000-01-01";
    document.getElementById("userType").checked = false;
    document.getElementById("userPhoto").value = [];
    var tempImg = document.getElementById("output");
    tempImg.src = "/x.jpg";
    //tempImg.rendered();
    //onchange="loadFile(event); elem.dispatchEvent(event)
}
// создание строки для таблицы
function row(user, edituserid) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", user.id);

    if (edituserid == user.id) { getUser(user.id) };

    const imgTd = document.createElement("td");

    var oImg = document.createElement("img");
    oImg.setAttribute('src', user.photo);
    oImg.setAttribute('alt', 'нет фото');
    oImg.setAttribute('height', '64px');
    oImg.setAttribute('width', '64px');
    imgTd.append(oImg);
    tr.append(imgTd);

    const nameTd = document.createElement("td");
    nameTd.append(user.name);
    tr.append(nameTd);


    //myDate.getDate() + "/" + (myDate.getMonth() + 1) + "/"  + myDate.getYear();

    const birthdayTd = document.createElement("td");
    //birthdayTd.append(user.birthday.slice(0, 10));
    dateBirthday = new Date(user.birthday.slice(0, 10));
    birthdayTd.append(dateBirthday.toLocaleDateString());
    tr.append(birthdayTd);

    const typeTd = document.createElement("td");
    var newCheckBox = document.createElement('input');
    newCheckBox.type = 'checkbox';
    newCheckBox.id = 'info' + user.id; // need unique Ids!
    newCheckBox.checked = user.type;
    newCheckBox.disabled = "disabled";
    typeTd.append(newCheckBox);
    tr.append(typeTd);

    const linksTd = document.createElement("td");

    const editLink = document.createElement("button");
    editLink.append("Изменить");
    editLink.addEventListener("click", async () => {
        if (document.getElementById("saveBtn") !== null) {
            await getUser(user.id)
        }
        else {
            window.location.replace(`/home/edit/?id=${user.id}`);
        }
    });
    linksTd.append(editLink);

    const removeLink = document.createElement("button");
    removeLink.append("Удалить");
    removeLink.addEventListener("click", async () => await deleteUser(user.id));

    linksTd.append(removeLink);
    tr.appendChild(linksTd);

    return tr;
}
// сброс значений формы
if (document.getElementById("resetBtn") !== null) {
    document.getElementById("resetBtn").addEventListener("click", () => reset());
}

function getBase64(file) {
    //console.log(file);
    if (typeof (file) == "undefined") {
        return ""
    }
    else {
        var reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onload = function () { //reader.onload
            console.log(reader.result);
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
        //console.log(temp);
        return reader.result;
    }
}

// отправка формы
if (document.getElementById("saveBtn") !== null) {
    document.getElementById("saveBtn").addEventListener("click", async () => {

        const id = document.getElementById("userId").value;
        const name = document.getElementById("userName").value;
        const birthday = new Date(document.getElementById("userBirthday").value).toJSON();
        const type = document.getElementById("userType").checked;

        //const photo = getBase64();

        const fileInput = document.getElementById("userPhoto").files[0];

        const photo = getBase64(fileInput);

        //document.getElementById("log2").innerHTML = photo;

        if (id === "")
            await createUser(name, birthday, type, photo);
        else
            await editUser(id, name, birthday, type, photo);
        reset();
    });
}
// birthday
// загрузка пользователей
getUsers();
