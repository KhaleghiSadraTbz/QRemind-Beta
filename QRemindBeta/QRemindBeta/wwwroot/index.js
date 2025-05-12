function addItem() {
    const a = document.getElementById('title').value;
    const b = document.getElementById('description').value;

    fetch(`/api/todo/add?Title=${a}&Desc=${b}`);

    document.getElementById('title').value = "";
    document.getElementById('description').value = "";

    location.reload();
}

function getAll() {
    const ul = document.getElementById('reminder-list');
    ul.replaceChildren();
    fetch("/api/todo/getall")
        .then(response => response.json())
        .then(json => {
            json.forEach((item, index) => {
                const li = document.createElement("li");
                li.classList.add('item');

                li.innerHTML = `
            <h2 class="item-title">${item.Title}</h2>
            <div class="item-content">
              <div class="item-description">
                ${item.Description}
              </div>
              <div class="item-actions">
                <div class="action-container">
                  <div class="checkbox-container">
                    <label class="checkbox-label">
                      Done <input type="checkbox" class="item-checkbox" ${item.IsDone ? "checked" : ""} onchange="checkchanged(this, ${index})">
                    </label>
                  </div>
                  <button class="item-button" onclick="deleteItem(${index})">Delete</button>
                </div>
              </div>
            </div>
          `;

                ul.appendChild(li);

            });
        }).catch(err => {
            console.error("Delete failed", err);
        });
            
}

getAll();

// Function to delete an item
function deleteItem(index) {
    const ul = document.getElementById('reminder-list');
    fetch(`/api/todo/del?Index=${index}`)
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                alert("Failed to delete item.");
            }
        })
        .catch(err => {
            console.error("Delete failed", err);
        });
}

function checkchanged(checkbox, index) {
    const isChecked = checkbox.checked;
    fetch(`/api/todo/setdone?Index=${index}&IsDone=${isChecked}`)
        .then(response => {
            if (response.ok) {
            } else {
                alert("Failed to delete item.");
            }
        })
        .catch(err => {
            console.error("Delete failed", err);
        });
}

document.getElementById('addItemForm').addEventListener('submit', function (event) {
    event.preventDefault(); // ✅ Prevent form from submitting normally
    addItem();              // ✅ Call your function
});

