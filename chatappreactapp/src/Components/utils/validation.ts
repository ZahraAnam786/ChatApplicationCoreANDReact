// utils/validation.ts
export function validateFields(values: Record<string, string>, rules: Record<string, Function>) {
    const errors: Record<string, string> = {};

    for (const field in rules) {
        const rule = rules[field];
        const error = rule(values[field], values); // pass value and full object for dependent checks
        if (error) {
            errors[field] = error;
        }
    }

    return errors;
}
