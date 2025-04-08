function sayHello() {
    alert("Hello!");
}
function sayGoodbye(Parameter) {
    alert("Goodbye!" + Parameter);
}
function setNameValue(executionContext) {
    const formContext = executionContext.getFormContext();
    const simpleStringAttr = formContext.getAttribute("theo_simplestring");
    const simpleStringValue = simpleStringAttr.getValue();

    const nameAttr = formContext.getAttribute("theo_name");
    nameAttr.setValue(simpleStringValue);
}
function myEnableRule() {
    // return true;
    return false;
}
