namespace fstomp

module ParserInternal = 
    open FParsec
    
    type UserState = unit
    
    type Parser<'t> = Parser<'t, UserState>
    
    let isStompChar c = c <> ':' && c <> '\r' && c <> '\n' && c <> (char 0)
    
    let scommand = 
        [ stringReturn "CONNECTED" fstomp.Connected
          stringReturn "MESSAGE" fstomp.Message
          stringReturn "RECEIPT" fstomp.Receipt
          stringReturn "ERROR" fstomp.Error ]
        |> choice
    
    let sheader = 
        let sheaderName = many1Satisfy isStompChar
        let sheaderValue = manySatisfy isStompChar
        sheaderName .>>. (pstring ":" >>. sheaderValue)
    
    let sheaderList = 

        let sheaderEol = sheader .>> newline
        many sheaderEol
    
    let sbody = manySatisfy (fun c -> c <> char 0) .>> (pchar (char 0))

    let spayload =         
        scommand .>> newline .>>. sheaderList .>> many newline .>>. sbody .>> many newline

    let sframe : Parser<_> = 
        let sprefix = pstring "a[\""
        let ssuffix = pstring "\"]"
        sprefix >>. spayload .>> ssuffix .>> eof     
    

module Parser = 
    open ParserInternal
    open FParsec
    
    // (Command * (string * string) list ) * string
    let Parse message = 
        let translated = 
            utils.translate [ ("\\n", "\n")
                              ("\\t", "\t") 
                              ("\\\"", "\"") 
                              ("\\r", "\r")
                              ("\\u0000", "\u0000") ] 
                              message
        match run sframe translated with
        | Success(((command, headers), body), _, _) -> 
            { ServerCommand = command
              Headers = 
                  List.map (fun (k, v) -> 
                      { Name = k
                        Value = v }) headers
              Body = body }
        | Failure(errorMsg, _, _) -> invalidArg "message" errorMsg
