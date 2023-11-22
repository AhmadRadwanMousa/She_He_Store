const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');

signUpButton.addEventListener('click', () => {
	container.classList.add("right-panel-active");
});

signInButton.addEventListener('click', () => {
	container.classList.remove("right-panel-active");
});
document.getElementById('resetPasswordLink').addEventListener('click', function () {
	document.getElementById('backdrop').style.display = 'flex';
	document.getElementById('backdrop').style.justifyContent = 'center';
	document.getElementById('backdrop').style.alignItems = 'center';
});

document.getElementById('close-backdrop').addEventListener('click', function () {
	document.getElementById('backdrop').style.display = 'none';
});