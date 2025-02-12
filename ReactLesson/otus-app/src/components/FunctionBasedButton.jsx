import './NewWayButton.css'

export function FunctionBasedButton(props) {
    return <>
        <button className="fancy-button"
            onClick={() => {
                if (props.onFancyClick)
                {
                    props.onFancyClick(new Date())
                }
            }}
        >

            New way button [{props.newText}]

        </button>

        after: {props.after}
    </>;
}