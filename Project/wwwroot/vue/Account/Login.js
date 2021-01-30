const vueApp = new Vue({
    el: '#vapp',
    data: {
        
        token: null,
        email: null,
        password: null,
        feedback: null,
        progressBar: null,
    },
    mounted() {
       // this.isTokenValid();
    },
    methods: {

        isTokenValid() {
           
            let token = localStorage.getItem("user-token");
          

            var headers = {
                    headers: {'Authorization': "Bearer " + token}
            };

            console.log(headers);

            axios
                .get(isTokenValidUrl, headers)
                .then(response => {
                    window.location.href = homeUrl;
                })
                .catch(error => {
                   // localStorage.removeItem('user-token');                   
                });;
        },


        doLogin() {
            
            if (!this.email) {
                this.feedback = "Please enter your email.";
                return false;
            }
            if (!this.password) {
                this.feedback = "Please enter your password.";
                return false;
            }

            let userDetails = {
                Email: this.email,
                Password: this.password
            };



            axios.post(userLoginUrl, userDetails)
                .then(response => {                    
                    //  this.token = response.data.token;
                    //localStorage.setItem("user-token", this.token);
                    
                    window.location.replace(userListUrl);

                })
                .catch(error => {
                    this.feedback = "פרטי ההתחברות שגויים";
                    //localStorage.removeItem("user-token");
                    this.progressBar = null;
                });

        },



    }
})