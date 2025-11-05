$(document).ready(function () {
    if (Notification.permission !== "granted") {
        Notification.requestPermission();
    }
});

var currentGroupId = 0;
var userId = 0;

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

connection.on("Welcome",
    function (id) {
        userId = id;
    });
connection.on("ReceiveMessage", receive);
connection.on("NewGroup", appendGroup);
connection.on("JoinGroup", joined);
connection.on("ReceiveNotification", sendNotification);

connection.on("Error", function (str) {
    alert(str)
});
async function Start() {
    try {
        await connection.start();
        $(".disConnect").hide();

    } catch (e) {
        $(".disConnect").show();
        setTimeout(Start, 6000);
    }
}

connection.onclose(async () => {
    await Start();
});
Start();

function appendGroup(groupName, token, imageName) {
    if (groupName === "Error") {
        alert("ERROR");
    } else {
        $(".rooms #user_groups ul").append(`
                                             <li onclick="joinInGroup('${token}')">
                                            ${groupName}
                                            <img src="/image/groups/${imageName}" />
                                            <span></span>
                                        </li>
                                            `);
        $('#exampleModal').modal('hide');
    }
}

function insertGroup(event) {
    event.preventDefault();
    var groupName = event.target[0].value;
    var imageFile = event.target[1].files[0];
    var formData = new FormData();
    formData.append("GroupName", groupName);
    formData.append("ImageFile", imageFile);
    $.ajax({
        url: "/home/CreateGroup",
        type: "post",
        data: formData,
        encytype: "multipart/form-data",
        processData: false,
        contentType: false
    });
}

function search() {
    var text = $("#search_input").val();
    if (text) {
        $("#search_result").show();
        $("#user_groups").hide();
        $.ajax({
            url: "/home/search?title=" + text,
            type: "get"
        }).done(function (data) {
            $("#search_result ul").html("");
            for (var i in data) {
                if (data[i].isUser) {
                    $("#search_result ul").append(`
                                 <li onclick="joinInPrivateGroup(${data[i].token})">
                                            ${data[i].title}
                                            <img src="/img/${data[i].imageName}" />
                                            <span></span>
                                        </li>
                        `);
                } else {
                    $("#search_result ul").append(`
                                 <li onclick="joinInGroup('${data[i].token}')">
                                            ${data[i].title}
                                            <img src="/image/groups/${data[i].imageName}" />
                                            <span></span>
                                        </li>

                        `);
                }
            }

        });
    } else {
        $("#search_result").hide();
        $("#user_groups").show();
    }
}

function sendNotification(chat) {
    if (Notification.permission === "granted") {
        if (Notification.permission !== "granted") {
            Notification.requestPermission().then(permission => {
                if (permission === "granted") {
                    console.log(chat.groupId);
                    console.log(currentGroupId);
                    if (currentGroupId !== chat.groupId) {
                        try {
                            var notification = new Notification(chat.userName + ` group: ${chat.groupName}`, {
                                body: chat.chatBody
                            });
                        } catch (error) {
                            console.error("Notification error:", error);
                        }

                    }
                }
            });
        }
    }
}

function receive(chat) {

    $("#messageText").val('');
    if (userId === chat.userId) {
        if (chat.fileAttach) {
            $(".chats").append(`
                 <div class="chat-me">
                     <div class="chat">
                          <span>${chat.userName}</span>
                              <p><a href='/files/${chat.fileAttach}'  target="_blank">${chat.chatBody}</a><p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div> `);
        } else {
            $(".chats").append(`
                 <div class="chat-me">
                     <div class="chat">
                          <span>${chat.userName}</span>
                              <p>${chat.chatBody}</p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div>
                                            `);
        }
    } else {
        if (chat.fileAttach) {
            $(".chats").append(`
                 <div class="chat-you">
                     <div class="chat">
                          <span>${chat.userName}</span>
                           <p><a href='/files/${chat.fileAttach}'  target="_blank">${chat.chatBody}</a><p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div> `);
        } else {
            $(".chats").append(`
                 <div class="chat-you">
                     <div class="chat">
                          <span>${chat.userName}</span>
                              <p>${chat.chatBody}</p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div>
                                            `);
        }
    }

}

function sendMessage(event) {
    event.preventDefault();
    var text = $("#messageText").val();
    var file = event.target[1].files[0];
    var formData = new FormData();
    formData.append("GroupId", currentGroupId);
    formData.append("FileAttach", file);
    formData.append("ChatBody", text);
    $.ajax({
        url: "/home/SendMessage",
        type: "post",
        data: formData,
        encytype: "multipart/form-data",
        processData: false,
        contentType: false
    }).done(function () {
        $(".footer form input[type=file]").val('');
    });
}

function joined(group, chats) {
    $(".header").css("display", "block");
    $(".footer").css("display", "block");
    $(".header h2").html(group.groupTitle);
    $(".header img").attr("src", `/image/groups/${group.imageName}`);
    currentGroupId = group.id;
    $(".chats").html("");
    for (var i in chats) {
        var chat = chats[i];
        console.log(chat);
        if (userId === chat.userId) {
            if (chat.fileAttach) {
                $(".chats").append(`
                 <div class="chat-me">
                     <div class="chat">
                          <span>${chat.userName}</span>
                            <p><a href='/files/${chat.fileAttach}'  target="_blank">${chat.chatBody}</a><p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div> `);
            } else {
                $(".chats").append(`
                 <div class="chat-me">
                     <div class="chat">
                          <span>${chat.userName}</span>
                              <p>${chat.chatBody}</p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div>
                                            `);
            }
        } else {
            if (chat.fileAttach) {
                $(".chats").append(`
                 <div class="chat-you">
                     <div class="chat">
                          <span>${chat.userName}</span>
                              <p><a href='/files/${chat.fileAttach}' target="_blank">${chat.chatBody}</a><p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div> `);
            } else {
                $(".chats").append(`
                 <div class="chat-you">
                     <div class="chat">
                          <span>${chat.userName}</span>
                              <p>${chat.chatBody}</p>
                                <span>${chat.createDate}</span>
                                 </div>
                           </div>
                                            `);
            }
        }
    }
}

function joinInGroup(token, groupId) {
    connection.invoke("JoinGroup", token, currentGroupId);
    $(`#${currentGroupId}`).removeClass("selected");
    $(`#${groupId}`).addClass("selected");
}

function joinInPrivateGroup(receiverId) {
    connection.invoke("JoinPrivateGroup", receiverId, currentGroupId);
}