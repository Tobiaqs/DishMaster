export class Validation {
    validateName(name) {
        return (typeof name === "string") && name.length >= 1 && name.length <= 32;
    }

    validateBounty(bounty) {
        return (typeof bounty === "number") && bounty >= 0 && bounty <= 10 && Math.floor(bounty) === Math.ceil(bounty);
    }

    validateEmail(email) {
        return (typeof email === "string") && /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(email);
    }

    validateForm(values) {
        let errors = {};

        if ("fullName" in values) {
            if (values.fullName.length < 1 || values.fullName.length > 32) {
                errors.fullName = "Naam is ongeldig";
            }
        }

        if ("name" in values) {
            if (!this.validateName(values.name)) {
                errors.name = "Naam is ongeldig";
            }
        }

        if ("bounty" in values) {
            if (!this.validateBounty(values.bounty) && !values.isNeutral) {
                errors.bounty = "Opbrengst is ongeldig";
            }
        }

        if ("email" in values) {
            if (!this.validateEmail(values.email)) {
                errors.email = "E-mailadres is ongeldig";
            }
        }

        if ("passwordAgain" in values) {
            if (values.passwordAgain.length > 0 && values.password !== values.passwordAgain) {
                errors.passwordAgain = "De wachtwoorden komen niet overeen";
            }
        }

        return errors;
    }

    static _instance;

    static getInstance() {
        if (!Validation._instance) {
            Validation._instance = new Validation();
        }
        return Validation._instance;
    }
}