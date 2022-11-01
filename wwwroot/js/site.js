// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {

    el_autohide = document.querySelector('.autohide');

    // add padding-top to bady (if necessary)
    navbar_height = document.querySelector('.navbar').offsetHeight;
    document.body.style.paddingTop = navbar_height + 'px';

    if (el_autohide) {
        var last_scroll_top = 0;
        window.addEventListener('scroll', function () {
            let scroll_top = window.scrollY;
            if (scroll_top < last_scroll_top) {
                el_autohide.classList.remove('scrolled-down');
                el_autohide.classList.add('scrolled-up');
            }
            else {
                el_autohide.classList.remove('scrolled-up');
                el_autohide.classList.add('scrolled-down');
            }
            last_scroll_top = scroll_top;
        });
        // window.addEventListener
    }
    // if

});

function likeIt(itemId, item, type) {
    var likeObj = {
        id: itemId,
        poc: item,
        acttype: type
    };

    var data = JSON.stringify(likeObj);

    $.ajax({
        type: 'POST',
        url: '/MyPosts/Like',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (result) {
            if (item == "post") {
                document.getElementById("likeCount_" + itemId).innerHTML = result.likeCount;
                document.getElementById("dislikeCount_" + itemId).innerHTML = result.dislikeCount;
                if (result.userLike) {

                    //$(".like_" + itemId).attr("src", "~/img/liked.svg");

                    document.getElementById("like_" + itemId).src = "/img/liked.svg"
                } else {
                    //$(".like_" + itemId).attr("src", "~/img/like.svg");

                    document.getElementById("like_" + itemId).src = "/img/like.svg"
                }
                if (result.userDislike) {
                    //$(".dislike_" + itemId).attr("src", "~/img/disliked.svg");

                    document.getElementById("dislike_" + itemId).src = "/img/disliked.svg"
                } else {
                    //$(".dislike_" + itemId).attr("src", "~/img/disliked.svg");

                    document.getElementById("dislike_" + itemId).src = "/img/dislike.svg"
                }
            }
        },
        error: function (xhr) {
            alert("Error " + xhr.responseText);
        }
    })
}

function commentIt(postId, commentId, comment, comItem) {
    var commentObj = {
        postId: postId,
        commentId: commentId,
        comment: comment
    };
    var res = null;
    var data = JSON.stringify(commentObj);

    $.ajax({
        type: 'POST',
        url: '/MyPosts/Comment',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (result) {
            var pathArray = window.location.pathname.split('/');

            var comDate = result.timeStamp.slice(0, 16).replace("T", " ").replace(/-/g, "/");
            var newDiv = document.createElement("div");
            newDiv.id = "com_"+result.id;
            newDiv.classList.add("bg-light", "py-1", "px-3", "my-2");
            newDiv.style = "border-radius: 15px;";
            newDiv.innerHTML = "<span class='small fw-bold text-muted'>" + result.author.firstName + " " + result.author.lastName + ": </span><span class='small text-muted'>" + comDate + "</span>";
            if (pathArray[1] == "MyPosts" && pathArray[2] == "Index") {
                newDiv.innerHTML += "<a class='text-decoration-none float-end mt-2' href='#' onclick='commentDelete("+result.postId+", "+result.id+")'> <img src='/img/remove.svg' /> </a>"
            }
            newDiv.innerHTML += "<br/><span>" + result.body + "</span>";

            comItem.parentNode.insertBefore(newDiv, comItem.nextSibling);

            var comCountEl = document.getElementById("commCount_" + postId);
            var comCount = parseInt(comCountEl.innerHTML) + 1;
            comCountEl.innerHTML = comCount;

        },
        error: function (xhr) {
            alert("Error " + xhr.responseText);
        }
    })
}

function commentDelete(postId, commentId) {
    var commentObj = {
        postId: postId,
        commentId: commentId,
        comment: "empty"
    };
    var res = null;
    var data = JSON.stringify(commentObj);
    let text = "Proceed with delete?";
    if (confirm(text) == true) {

    $.ajax({
        type: 'POST',
        url: '/MyPosts/CommentDel',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (result) {
            if (result) {
                var comCountEl = document.getElementById("commCount_" + postId);
                var comCount = parseInt(comCountEl.innerHTML) - 1;
                comCountEl.innerHTML = comCount;
                document.getElementById("com_" + commentId).remove();
            }
        },
        error: function (xhr) {
            alert("Error " + xhr.responseText);
        }
    })
    }
}


$(".commentArea").keydown(function (event) {
    if (event.keyCode === 13 && !event.shiftKey) {
        event.preventDefault();
        var postId = this.parentNode.id.split("_")[1];
        commentIt(postId, 0, this.value, this);
        this.value = "";
        this.rows = "1";

    } else {
        const text = this.value;
        const lines = text.split("\n");
        const count = lines.length;
        this.rows = count.toString();
    }
});

