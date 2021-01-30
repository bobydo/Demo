<div id="app_university" class="mb-3">
    <b-alert show>Demo for university CRUD</b-alert>
    <div class="card">
        <div class="card-header">
            <div class="d-flex justify-content-between">
                <h3 class="mb-0">University</h3>
                <button class="btn btn-secondary" @click="() => openModal()">Create</button>
            </div>
        </div>
        <div class="card-body">
            <div v-if="universities.length > 0">
                <table class="table mb-0">
                    <thead class="table-dark">
                    <tr>
                        <th v-for="(_, key) in universities[0]" :key="key">
                            {{ key }}
                        </th>
                        <th>Operations</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(eachUniversity, index) in universities" :key="eachUniversity.id">
                        <td>{{ eachUniversity['id'] }}</td>
                        <td>{{ eachUniversity['name'] }}</td>
                        <td>
                            <button class="btn btn-primary" @click="() => openModal(eachUniversity)">Edit</button>
                            <button class="btn btn-danger" @click="() => removeUniversity(eachUniversity['id'], index)">Remove</button>
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
                        v-model="currentUniversity.name"
                        :state="nameState"
                        aria-describedby="input-live-help input-live-feedback"
                        placeholder="Enter the university's name"
                        trim
                ></b-form-input>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Minimum is 1 letter, Maximum is 30 letters
                </b-form-invalid-feedback>
            </div>
        </div>

        <template #modal-footer="{ cancel }">
            <b-button variant="danger" @click="cancel()">
                Cancel
            </b-button>
            <b-button v-if="isCreateMode" variant="primary" @click="createUniversity">
                Create
            </b-button>
            <b-button v-else variant="primary" @click="updateUniversity">
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
        el: '#app_university',
        data() {
            return {
                params: `%%params%%`,
                universities: [],
                currentUniversity: {
                    "name": "",
                },
                isCreateMode: true,
                isLoading: false
            }
        },
        computed: {
            nameState() {
                return this.currentUniversity['name'].length < 30 && this.currentUniversity['name'].length > 1;
            },
            salesModalTitle() {
                return this.isCreateMode ? 'Create a university' : 'Edit the university';
            }
        },
        mounted() {
            UniversityService.getUniversity().then(universities => {
                this.universities = universities;
            });
        },
        methods: {
            openModal(university=null) {
                if(university==null) {
                    this.currentUniversity = {
                        "name": ""
                    };
                    this.isCreateMode = true;
                } else {
                    this.currentUniversity = JSON.parse(JSON.stringify(university));
                    this.isCreateMode = false;
                }
                this.$refs['salesModal'].show();
            },
            createUniversity() {
                if(!(this.nameState))
                    return;
                this.isLoading = true;
                UniversityService.createUniversity(this.currentUniversity).then(university => {
                    this.universities.push(university);
                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Create a university successfully',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                }).catch(reason => {
                    Swal.fire({
                        title: 'Failed!',
                        text: 'Error occurs, reason: ' + reason.response,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                    console.log(reason);
                }).finally(() => {
                    this.isLoading = false;
                    this.$refs['salesModal'].hide();
                });
            },
            removeUniversity(universityId, index) {
                this.isLoading = true;
                UniversityService.deleteUniversity(universityId).then(() => {
                    this.universities.splice(index, 1);
                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Delete a university successfully',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                }).catch(reason => {
                    Swal.fire({
                        title: 'Failed!',
                        text: 'Error occurs, reason: ' + reason.response,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }).finally(() => {
                    this.isLoading = false;
                });
            },
            updateUniversity() {
                if(!(this.nameState))
                    return;
                this.isLoading = true;
                UniversityService.updateUniversity(this.currentUniversity['id'], this.currentUniversity).then((university) => {
                    let index = Utility.indexOfObjArray(this.universities, 'id', university['id']);
                    this.$set(this.universities, index, university);

                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Update the university successfully',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                }).catch(reason => {
                    Swal.fire({
                        title: 'Failed!',
                        text: 'Error occurs, reason: ' + reason.response,
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