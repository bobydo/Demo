let rootUrl = '';
let apiUrl = '';

function init(baseUrl) {
    rootUrl = baseUrl;
    apiUrl = rootUrl + '/wp-json/demo/v1/';
}

class SalesService {
    static getSales() {
        return new Promise((resolve, reject) => {
            axios.get(apiUrl + 'sales').then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static createSales(sales) {
        return new Promise((resolve, reject) => {
            axios.post(apiUrl + 'sales', sales).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static updateSales(salesId, sales) {
        let url = apiUrl + 'sales?id=' + salesId;
        return new Promise((resolve, reject) => {
            axios.put(url, sales).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static deleteSales(salesId) {
        let url = apiUrl + 'sales?id=' + salesId;
        return new Promise((resolve, reject) => {
            axios.delete(url).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }
}

class StudentService {
    static getStudent() {
        return new Promise((resolve, reject) => {
            axios.get(apiUrl + 'student').then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static createStudent(student) {
        delete student['sales'];
        delete student['university'];
        return new Promise((resolve, reject) => {
            axios.post(apiUrl + 'student', student).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static updateStudent(studentId, student) {
        delete student['sales'];
        delete student['university'];
        let url = apiUrl + 'student?id=' + studentId;
        return new Promise((resolve, reject) => {
            axios.put(url, student).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static deleteStudent(studentId) {
        let url = apiUrl + 'student?id=' + studentId;
        return new Promise((resolve, reject) => {
            axios.delete(url).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }
}

class UniversityService {
    static getUniversity() {
        return new Promise((resolve, reject) => {
            axios.get(apiUrl + 'university').then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static createUniversity(university) {
        return new Promise((resolve, reject) => {
            axios.post(apiUrl + 'university', university).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static updateUniversity(universityId, university) {
        let url = apiUrl + 'university?id=' + universityId;
        return new Promise((resolve, reject) => {
            axios.put(url, university).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }

    static deleteUniversity(universityId) {
        let url = apiUrl + 'university?id=' + universityId;
        return new Promise((resolve, reject) => {
            axios.delete(url).then(response => {
                resolve(response.data);
            }).catch(reason => {
                reject(reason);
            });
        });
    }
}
