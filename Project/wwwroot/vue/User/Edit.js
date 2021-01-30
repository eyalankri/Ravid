const vueApp = new Vue({
    el: '#vapp',
    data: {                        
        password: null,
        feedback: null,
        userId: userId,
    },
    mounted() {
         
        
    },
    methods: {
 

        changePassword() {
            
            
            if (!this.password) {
                this.feedback = "יש לרשום סיסמה חדשה";
                return false;
            }

            let details = {                
                Password: this.password,
                UserId: this.userId
            };

           
            axios.post(changePasswordUrl, details)
                .then(response => {   
                    this.feedback = "הסיסמה נשמרה בהצלחה";
                   

                })
                .catch(error => {
                    this.feedback = "שגיאה! הנתונים לא נשמרו";                   
                });

        },



    }
})