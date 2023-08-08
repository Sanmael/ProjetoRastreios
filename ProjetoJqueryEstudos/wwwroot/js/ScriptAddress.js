function createAddressInput() {
    const addressForm = document.getElementById('addressForm');

    const newAddressDiv = document.createElement('div');
    newAddressDiv.className = 'address';

    const postalCodeInput = createInput('text', 'PostalCode', 'CEP');
    const cityInput = createInput('text', 'City', 'Cidade', true);
    const stateInput = createInput('text', 'State', 'Estado', true);
    const neighborhoodInput = createInput('text', 'Neighborhood', 'Bairro', true);
    const publicPlaceInput = createInput('text', 'PublicPlace', 'Rua', true);
    const isPrincipalCheckbox = createInput('checkbox', 'isPrincipalAddress', 'isPrincipalAddress');


    isPrincipalCheckbox.addEventListener('change', handleIsPrincipalCheckbox);

    function resetForm() {
        cityInput.value = "";
        stateInput.value = "";
        neighborhoodInput.value = "";
        publicPlaceInput.value = "";
    }

    postalCodeInput.addEventListener('blur', function () {
        const cep = this.value.replace(/\D/g, '');

        if (cep !== '') {
            const validateCep = /^[0-9]{8}$/;

            if (validateCep.test(cep)) {
                fetch(`https://viacep.com.br/ws/${cep}/json/`)
                    .then(response => response.json())
                    .then(data => {
                        if (!data.erro) {
                            cityInput.value = data.localidade;
                            stateInput.value = data.uf;
                            neighborhoodInput.value = data.bairro;
                            publicPlaceInput.value = data.logradouro;
                        } else {
                            resetForm();
                            alert("CEP não encontrado.");
                        }
                    })
                    .catch(error => {
                        resetForm();
                        alert("Ocorreu um erro ao buscar o CEP.");
                    });
            } else {
                resetForm();
                alert("Formato de CEP inválido.");
            }
        } else {
            resetForm();
        }
    });

    newAddressDiv.appendChild(postalCodeInput);
    newAddressDiv.appendChild(cityInput);
    newAddressDiv.appendChild(stateInput);
    newAddressDiv.appendChild(neighborhoodInput);
    newAddressDiv.appendChild(publicPlaceInput);
    newAddressDiv.appendChild(isPrincipalCheckbox);

    addressForm.appendChild(newAddressDiv);
}
function handleIsPrincipalCheckbox() {
    const checkboxes = document.querySelectorAll('input[type="checkbox"][name="isPrincipalAddress"]');
    checkboxes.forEach(checkbox => {
        if (checkbox !== this) {
            checkbox.checked = false;
        }
    });
}

function createInput(type, name, placeholder, readonly = false) {
    const input = document.createElement('input');
    input.type = type;
    input.name = name;
    input.placeholder = placeholder;
    input.required = true;
    if (readonly) {
        input.setAttribute('readonly', 'readonly');
    }
    return input;

}


function getAddressesData() {
    const addresses = [];
    const addressDivs = document.querySelectorAll('.address');

    addressDivs.forEach((addressDiv) => {
        const inputs = addressDiv.querySelectorAll('input');
        const addressData = {};

        inputs.forEach((input) => {
            if (input.type === 'checkbox') {
                addressData[input.name] = input.checked; 
            } else {
                addressData[input.name] = input.value;
            }
        });

        addresses.push(addressData);
    });

    return addresses;
}

async function saveAddresses() {
    const addresses = getAddressesData();
    const personId = document.getElementById('PersonId').value;
    console.log(addresses);
    console.log(personId);

    try {
        const response = await fetch(`/Person/SaveAddresses?personId=${personId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(addresses),
        });

        if (response.ok) {
            alert('Endereços salvos com sucesso!');
        } else {
            alert('Ocorreu um erro ao salvar os endereços.');
        }
    } catch (error) {
        console.error('Erro ao enviar os dados:', error);
        alert('Ocorreu um erro ao salvar os endereços.');
    }
}



document.getElementById('addAddressBtn').addEventListener('click', createAddressInput);
document.getElementById('saveBtn').addEventListener('click', saveAddresses);
