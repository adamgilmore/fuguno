function formatString(str, placeholder) {
    if (_.isNull(str) || str.isBlank()) return placeholder;
    return str;
}

function formatTime(str, placeholder) {
    if (_.isNull(str) || str.isBlank()) return placeholder;
    return moment.duration(str).humanize();
}

function formatDate(date, placeholder) {
    if (_.isNull(date) || (_.isString(date) && date.isBlank())) return placeholder;
    return moment(date).fromNow();
}

function formatTestPassPercentage(testPassPercentage) {
    if (testPassPercentage == null) return "";
    return testPassPercentage.round(0) + "%";
}

function getTestPassPercentage(totalTestCount, totalTestPassedCount) {
    if (_.isNaN(totalTestCount) || _.isNaN(totalTestPassedCount)) return null;
    if (totalTestPassedCount == 0 || totalTestCount == 0) return null;
    return (totalTestPassedCount / totalTestCount) * 100;
}