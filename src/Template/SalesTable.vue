<div id="app_sales" class="mb-3">
    <b-alert show>Demo for sales CRUD</b-alert>
    <div class="card">
        <div class="card-header">
            <div class="d-flex justify-content-between">
                <h3 class="mb-0">Sales</h3>
                <button class="btn btn-secondary" @click="() => openModal()">Create</button>
            </div>
        </div>
        <div class="card-body">
            <div v-if="sales.length > 0">
                <table class="table mb-0">
                    <thead class="table-dark">
                    <tr>
                        <th v-for="(_, key) in sales[0]" :key="key">
                            {{ key }}
                        </th>
                        <th>Operations</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="(eachSales, index) in sales" :key="eachSales.id">
                        <td>{{ eachSales['id'] }}</td>
                        <td>{{ eachSales['first_name'] }}</td>
                        <td>{{ eachSales['last_name'] }}</td>
                        <td>{{ eachSales['email'] }}</td>
                        <td>
                            {{ eachSales['students'].map(student => student['first_name'] + ' ' + student['last_name']).join(', ') }}
                        </td>
                        <td>
                            <button class="btn btn-primary" @click="() => openModal(eachSales)">Edit</button>
                            <button class="btn btn-danger" @click="() => removeSales(eachSales['id'], index)">Remove</button>
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
                        v-model="currentSales.first_name"
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
                        v-model="currentSales.last_name"
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
                <label>Email:</label>
                <b-form-input
                        v-model="currentSales.email"
                        :state="resumeState"
                        aria-describedby="input-live-help input-live-feedback"
                        placeholder="Enter your email"
                        trim
                ></b-form-input>

                <!-- This will only be shown if the preceding input has an invalid state -->
                <b-form-invalid-feedback id="input-live-feedback">
                    Invalid Email Address
                </b-form-invalid-feedback>
            </div>
        </div>

        <template #modal-footer="{ cancel }">
            <b-button variant="danger" @click="cancel()">
                Cancel
            </b-button>
            <b-button v-if="isCreateMode" variant="primary" @click="createSales">
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
        el: '#app_sales',
        data() {
            return {
                params: `%%params%%`,
                sales: [],
                currentSales: {
                    "first_name": "",
                    "last_name": "",
                    "email": ""
                },
                isCreateMode: true,
                isLoading: false
            }
        },
        computed: {
            firstNameState() {
                return this.currentSales['first_name'].length < 30 && this.currentSales['first_name'].length > 1;
            },
            lastNameState() {
                return this.currentSales['last_name'].length < 30 && this.currentSales['last_name'].length > 1;
            },
            resumeState() {
                return this.currentSales['email'].includes('@') && this.currentSales['email'].includes('.');
            },
            salesModalTitle() {
                return this.isCreateMode ? 'Create a sales' : 'Edit the sales';
            }
        },
        mounted() {
            SalesService.getSales().then(sales => {
                this.sales = sales;
            });
        },
        methods: {
            openModal(sales=null) {
                if(sales==null) {
                    this.currentSales = {
                        "first_name": "",
                        "last_name": "",
                        "email": ""
                    };
                    this.isCreateMode = true;
                } else {
                    this.currentSales = JSON.parse(JSON.stringify(sales));
                    this.isCreateMode = false;
                }
                this.$refs['salesModal'].show();
            },
            createSales() {
                if(!(this.firstNameState && this.lastNameState && this.resumeState))
                    return;
                this.isLoading = true;
                SalesService.createSales(this.currentSales).then(sales => {
                    this.sales.push(sales);
                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Create a sales successfully',
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
            removeSales(salesId, index) {
                this.isLoading = true;
                SalesService.deleteSales(salesId).then(() => {
                    this.sales.splice(index, 1);
                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Delete a sales successfully',
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
            updateSales() {
                if(!(this.firstNameState && this.lastNameState && this.resumeState))
                    return;
                this.isLoading = true;
                SalesService.updateSales(this.currentSales['id'], this.currentSales).then((sales) => {
                    let index = Utility.indexOfObjArray(this.sales, 'id', sales['id']);
                    this.$set(this.sales, index, sales);

                    Swal.fire({
                        title: 'Succeed!',
                        text: 'Update the sales successfully',
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