
function createOption(i){
    let variant = document.createElement("input");
    variant.id = i + 1;
    variant.type = "text";
    variant.name = "[" + (i + 1) + "]";
    variant.placeholder = "Type option...";
    return variant;
}
function normalize() {
    let elem = document.getElementById("variants");
    let size = elem.childElementCount;
    for (var i = 0; i < size; i++) {
        elem.children[i].id = i;
        elem.children[i].name = "[" + i + "]";
    }
}
$(".input-panel.add-edit").on('input', 'input', function (e) {
    let elem = document.getElementById("variants");
    let size = elem.childElementCount;
    let option = e.target;
    if (option.id < size - 1 && option.value == "") {
        elem.removeChild(option);
        normalize();
    }
    else if (option.id == size - 1 && option.value != "") {
        elem.appendChild(createOption(parseInt(option.id)));
        normalize();
    }
});

$(".poll-area").on('click', 'label', function () {
    if (!$('.poll-area').hasClass('voted')) {
        let inputs = document.getElementsByName("variant");
        let labels = document.getElementsByClassName("opt");

        for (var i = 0; i < inputs.length; i++) {
            labels[i].classList.remove("selected");
        }
        $(this).addClass("selected");
        document.getElementById("input_" + this.id).checked = true;
    }
});


$('form[name = "voting"]').on('submit', function () {
    setTimeout(() => { 
    let labels = document.getElementsByClassName("opt");
    for (var i = 0; i < labels.length; i++) {
        labels[i].classList.add("selectall");
        }
    }, 1000);
});

$(".overlay").on("click", function openfileDialog() {
    document.getElementById("fileLoader").click();
})
$("#fileLoader").on("change", function () {
    setTimeout(() => { $("#uploadimage").submit(); }, 500);
    
})


let v = document.getElementsByClassName("voters-container");
for (var i = 0; i < v.length; i++) {
    if (v[i].children.length > 0) {
        v[i].classList.remove("hidden");
    }
}
function resizeInput() {
    $(this).attr('size', $(this).val().length);
}

$('#nickname').keyup(resizeInput).each(resizeInput);

$("#change_nick").on("click", function () {
    let nick = $('#nickname')[0];
    console.log(nick.value);
    document.getElementById("nickname").setSelectionRange(nick.value.length, nick.value.length);
    $('#nickname').focus();
})
$('#nickname').on("focus", "")

