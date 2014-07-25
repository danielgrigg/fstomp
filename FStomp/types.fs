namespace fstomp
   
    type Ack = 
        | Auto
        | Client
        | ClientIndividual

    type ServerCommand =                 
        | Connected
        | Message
        | Receipt
        | Error


        // Unfortunately f#3.0 lacks named fields in discriminated unions.
        // So we must write value constructors like: 'Begin of string',
        // whereas in f#3.1 we can write, 'Begin of transactionId : string'.
        // This extra syntax is also used by Intellisense 
        // for eg, parameter completion.
    type ClientCommand =
    
        // | Send of channel : string * body : string
        | Send of string * string 

        //| Subscribe of subscriptionId : string * channel : string         
        | Subscribe of string * string

        //| Unsubscribe of subscriptionId : string
        | Unsubscribe of string

        //| Begin of transactionId : string
        | Begin of string

        //| Commit of transactionId : string
        | Commit of string

        //| Abort of transactionId : string
        | Abort of string

        //| Ack of messageId : string * transactionId : string
        | Ack of string * string

        //| Nack of messageId : string * transactionId : string
        | Nack of string * string

        //| Disconnect  // receipt ??
        | Disconnect  // receipt ??

        //| Connect of hostname : string * heartbeat : bool
        | Connect of string * bool
                
        | Stomp // Error type - not documented in http://stomp.github.io/stomp-specification-1.2.html


    type Header = { Name : string; Value : string }

        
    type ClientMessage = 
        { ClientCommand: ClientCommand; Headers: Header seq; Body : string }
        override this.ToString() = sprintf "%A %A Body %s" this.ClientCommand this.Headers this.Body

    
    type ServerMessage = 
        { ServerCommand: ServerCommand; Headers: Header seq; Body : string }
        override this.ToString() = sprintf "%A %A Body %s" this.ServerCommand this.Headers this.Body