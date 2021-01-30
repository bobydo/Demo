<div id="app_student" class="mb-3">
    <b-alert show>Demo for student CRUD</b-alert>
    <div class="card">
        <div class="card-header">
            <div class="d-flex justify-content-between">
                <h3 class="mb-0">Student</h3>
                <button class="btn btn-secondary" @click="() => openModal()">Create</button>
            </div>
        </div>
        <div class="card-body">
            <div v-if="students.length > 0">
                <table class="table mb-0">
                    <thead class="table-dark">
                    <tr>
                        <th>id</th>
                        <th>first_name</th>
                        <th>last_name</th>
                        <th>university</th>
                        <th>resume</th>
                        <th>sales</th>
                        <th>Operations</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(eachStudent, index) in students" :key="eachStudent.id">
                        <td>{{ eachStudent['id'] }}</td>
                        <td>{{ eachStudent['first_name'] }}</td>
                        <td>{{ eachStudent['last_name'] }}</td>
                        <td>{{ ('university' in eachStudent) ? eachStudent['university']['name'] : ''}}</td>
                        <td><a :href="eachStudent['resume']" target="_blank">Open resume</a></td>
                        <td>
                            {{ ('sales' in eachStudent)
                                ? eachStudent['sales']['first_name'] + ' ' + eachStudent['sales']['last_name']
                                : ''
                            }}
                        </td>
                        <td>
                            <button class="btn btn-primary" @click="() => openModal(eachStudent)">Edit</button>
                            <button class="btn btn-danger" @click="() => removeSales(eachStudent['id'], index)">Remove</button>
                        </td>
                    </tr>
                    </tbody>
                </table>
            </div>
            <div v-else>
                <b-alert class="mb-0" variant="danger" show>No sales data</b-alert>
            </div>
        </div>
    </div>

    <b-modal id="sales-modal" ref="salesModal" centered scrollable :title="salesModalTitle">
        <div>
            <div role="group">
                <label>First Name:</label>
                <b-form-input
                        v-model="currentStudent.first_name"
                        :state="firstNameState"
                        aria-describedby="input-live-help input-live-feedback"
                        placeholder="Enter your first name"
                        trim
                ></b-form-input>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Minimum is 1 letter, Maximum is 30 letters
                </b-form-invalid-feedback>
            </div>
            <div role="group">
                <label>Last Name:</label>
                <b-form-input
                        v-model="currentStudent.last_name"
                        :state="lastNameState"
                        aria-describedby="input-live-help input-live-feedback"
                        placeholder="Enter your last name"
                        trim
                ></b-form-input>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Minimum is 1 letter, Maximum is 30 letters
                </b-form-invalid-feedback>
            </div>
            <div role="group">
                <label>Resume:</label>
                <b-form-file v-if="isEditResume"
                        accept="application/pdf"
                        v-model="currentStudent.resume"
                        :state="resumeState"
                        placeholder="Choose a file or drop it here..."
                        drop-placeholder="Drop file here..."
                ></b-form-file>
                <div v-else>
                    <a class="btn btn-primary" :href="currentStudent.resume" target="_blank">Open resume</a>
                    <b-button @click="reUploadResume">Re-upload resume</b-button>
                </div>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Please upload the resume
                </b-form-invalid-feedback>
            </div>
            <div role="group">
                <label>University:</label>
                <b-form-select
                        :options="universityOptions"
                        v-model="currentStudent.university_id"
                        :state="universityState"
                        aria-describedby="input-live-help input-live-feedback"
                        placeholder="Enter your university"
                        trim
                ></b-form-select>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Please select a university
                </b-form-invalid-feedback>
            </div>
            <div role="group">
                <label>Sales:</label>
                <b-form-select
                        :options="salesOptions"
                        v-model="currentStudent.sales_id"
                        :state="salesState"
                        aria-describedby="input-live-help input-live-feedback"
                        placeholder="Enter your sales"
                        trim
                ></b-form-select>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Please select a sales
                </b-form-invalid-feedback>
            </div>
        </div>

        <template #modal-footer="{ cancel }">
            <b-button variant="danger" @click="cancel()">
                Cancel
            </b-button>
            <b-button v-if="isCreateMode" variant="primary" @click="createStudent">
                Create
            </b-button>
            <b-button v-else variant="primary" @click="updateSales">
                Edit
            </b-button>
        </template>
    </b-modal>

    <div class="loading" v-if="isLoading">
        <div class="sk-chase">
            <div class="sk-chase-dot"></div>
            <div class="sk-chase-dot"></div>
            <div class="sk-chase-dot"></div>
            <div class="sk-chase-dot"></div>
            <div class="sk-chase-dot"></div>
            <div class="sk-chase-dot"></div>
        </div>
    </div>
</div>

<script>
    var app = new Vue({
        el: '#app_student',
        data() {
            return {
                params: `%%params%%`,
                students: [],
                sales: [],
                universities: [],
                currentStudent: {
                    "first_name": "",
                    "last_name": "",
                    "university_id": null,
                    "resume": "",
                    "sales_id": null
                },
                isCreateMode: true,
                isEditResume: true,
                isLoading: false
            }
        },
        computed: {
            firstNameState() {
                return this.currentStudent['first_name'].length < 30 && this.currentStudent['first_name'].length > 1;
            },
            lastNameState() {
                return this.currentStudent['last_name'].length < 30 && this.currentStudent['last_name'].length > 1;
            },
            salesModalTitle() {
                return this.isCreateMode ? 'Create a students' : 'Edit the students';
            },
            universityState() {
                return this.currentStudent['university_id'] !== null;
            },
            salesState() {
                return this.currentStudent['sales_id'] !== null;
            },
            salesOptions() {
                let result = [{ value: null, text: "Please select a sales" }];
                for(let eachSales of this.sales) {
                    result.push({ value: eachSales['id'], text: eachSales['first_name'] + ' ' + eachSales['last_name'] });
                }
                return result;
            },
            resumeState() {
                return this.currentStudent['resume'] !== null;
            },
            universityOptions() {
                let result = [{ value: null, text: "Please select a university" }];
                for(let eachUniversity of this.universities) {
                    result.push({ value: eachUniversity['id'], text: eachUniversity['name'] });
                }
                return result;
            }
        },
        mounted() {
            StudentService.getStudent().then(students => {
                for(let student of students) {
                    student['university_id'] = student['university']['id'];
                    student['sales_id'] = student['sales']['id'];
                    this.students.push(student);
                }
                this.students = students;
                return SalesService.getSales();
            }).then(sales => {
                this.sales = sales;
                return UniversityService.getUniversity();
            }).then(universities => {
                this.universities = universities;
            });
        },
        methods: {
            openModal(student=null) {
                if(student==null) {
                    this.currentStudent = {
                        "first_name": "",
                        "last_name": "",
                        "university_id": null,
                        "resume": null,
                        "sales_id": null
                    };
                    this.isCreateMode = true;
                    this.isEditResume = true;
                } else {
                    this.currentStudent = JSON.parse(JSON.stringify(student));
                    this.isCreateMode = false;
                    this.isEditResume = false;
                }
                this.$refs['salesModal'].show();
            },
            reUploadResume() {
                this.currentStudent['resume'] = null;
                this.isEditResume = true;
            },
            async createStudent() {
                if(!(this.firstNameState && this.lastNameState && this.universityState && this.salesState && this.resumeState))
                    return;
                this.isLoading = true;
                const currentStudent = Object.assign({}, this.currentStudent);
                currentStudent.resume = await Utility.toBase64(currentStudent.resume);
                StudentService.createStudent(currentStudent).then(student => {
                    this.students.push(student);
                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Create a student successfully',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                }).catch(reason => {
                    Swal.fire({
                        title: 'Failed!',
                        text: 'Error occurs, reason: ' + reason.response['message'],
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                    console.log(reason);
                }).finally(() => {
                    this.isLoading = false;
                    this.$refs['salesModal'].hide();
                });
            },
            removeSales(studentId, index) {
                this.isLoading = true;
                StudentService.deleteStudent(studentId).then(() => {
                    this.students.splice(index, 1);
                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Delete a student successfully',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                }).catch(reason => {
                    Swal.fire({
                        title: 'Failed!',
                        text: 'Error occurs, reason: ' + reason.response['message'],
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }).finally(() => {
                    this.isLoading = false;
                });
            },
            async updateSales() {
                if(!(this.firstNameState && this.lastNameState && this.universityState && this.salesState && this.resumeState))
                    return;
                this.isLoading = true;

                const currentStudent = Object.assign({}, this.currentStudent);
                if(typeof(currentStudent.resume) !== "string")
                    currentStudent.resume = await Utility.toBase64(currentStudent.resume);
                StudentService.updateStudent(currentStudent['id'], currentStudent).then((student) => {
                    student['university_id'] = student['university']['id'];
                    student['sales_id'] = student['sales']['id'];
                    let index = Utility.indexOfObjArray(this.students, 'id', student['id']);
                    this.$set(this.students, index, student);

                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Update the student successfully',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                }).catch(reason => {
                    Swal.fire({
                        title: 'Failed!',
                        text: 'Error occurs, reason: ' + reason.response['message'],
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                    console.log(reason);
                }).finally(() => {
                    this.isLoading = false;
                    this.$refs['salesModal'].hide();
                });
            }
        }
    });
</script>